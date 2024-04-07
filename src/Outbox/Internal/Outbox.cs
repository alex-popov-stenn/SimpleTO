using Dapper;
using System.Collections.Immutable;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serialization;

namespace Outbox.Internal
{
    internal sealed class Outbox : IOutbox
    {
        private readonly DbContext _dbContext;
        private readonly ISerializer _serializer;
        private const string InsertInOutboxQueryName = "InsertInOutbox.sql";
        private const string ReservedForProcessingQueryName = "ReserveForProcessing.sql";
        private const string MarkAsProcessedQueryName = "MarkAsProcessed.sql";
        private const string DeleteProcessedQueryName = "DeleteProcessed.sql";

        public Outbox(DbContext dbContext, ISerializer serializer)
        {
            _dbContext = dbContext;
            _serializer = serializer;
        }

        public async Task AddAsync<T>(T data, string topic, Func<T, string>? partitionBy, bool isSequential, Dictionary<string, string>? metadata, CancellationToken cancellationToken) 
            where T : class
        {
            var transaction = GetTransaction();
            var connection = transaction.Connection;
            var query = SqlQueriesReader.ReadWithCache(InsertInOutboxQueryName);
            var json = _serializer.Serialize(data);

            var commandDefinition = new CommandDefinition(query, new
            {
                RawData = json,
                MessageType = GetMessageTypeName(data),
                Topic = topic,
                PartitionBy = partitionBy?.Invoke(data) ?? null,
                IsSequential = isSequential,
                Metadata = metadata != null ? _serializer.Serialize(metadata) : null
            }, cancellationToken: cancellationToken, transaction: transaction);

            await connection.ExecuteAsync(commandDefinition);
        }

        public async Task<ImmutableArray<OutboxRecord>> ReserveAsync(int top, TimeSpan reservationTimeout,
            CancellationToken cancellationToken)
        {
            var query = SqlQueriesReader.ReadWithCache(ReservedForProcessingQueryName);
            return await GetByQueryAsync(query, cancellationToken,
                new { MaxLimit = top, ReservationSeconds = reservationTimeout.Seconds });
        }

        public async Task MarkAsProcessedAsync(ImmutableArray<OutboxRecord> data, CancellationToken cancellationToken)
        {
            var transaction = GetTransaction();
            var connection = transaction.Connection;
            var query = SqlQueriesReader.ReadWithCache(MarkAsProcessedQueryName);
            foreach (var i in data.Select(p => p.Id).Chunk(500))
            {
                var commandDefinition = new CommandDefinition(query, new { Ids = i },
                    cancellationToken: cancellationToken, transaction: transaction);
                await connection.ExecuteAsync(commandDefinition);
            }
        }

        public async Task DeleteProcessedAsync(CancellationToken cancellationToken)
        {
            var transaction = GetTransaction();
            var connection = transaction.Connection;
            var query = SqlQueriesReader.ReadWithCache(DeleteProcessedQueryName);
            var commandDefinition = new CommandDefinition(query, cancellationToken: cancellationToken,
                transaction: transaction);
            await connection.ExecuteAsync(commandDefinition);
        }

        private async Task<ImmutableArray<OutboxRecord>> GetByQueryAsync(string query,
            CancellationToken cancellationToken, object? queryData = null)
        {
            var transaction = GetTransaction();
            var connection = transaction.Connection;
            var commandDefinition = new CommandDefinition(query, queryData, cancellationToken: cancellationToken,
                transaction: transaction);
            var rows = await connection
                .QueryAsync<(Guid Id, DateTimeOffset DateTimestamp, string RawData, string
                    MessageType, string Topic, string? PartitionBy, bool IsProcessed, bool IsSequential, string Metadata)>(commandDefinition);

            var builder = ImmutableArray.CreateBuilder<OutboxRecord>();

            foreach (var r in rows)
            {
                var metadata = string.IsNullOrEmpty(r.Metadata)
                    ? null
                    : _serializer.Deserialize<Dictionary<string, string>>(r.Metadata);

                var outboxRecord = new OutboxRecord
                {
                    Id = r.Id,
                    Timestamp = r.DateTimestamp,
                    MessageType = r.MessageType,
                    Topic = r.Topic,
                    PartitionBy = r.PartitionBy,
                    IsProcessed = r.IsProcessed,
                    Metadata = metadata,
                    JsonRawData = r.RawData
                };

                builder.Add(outboxRecord);
            }

            return builder.ToImmutable();
        }

        private DbTransaction GetTransaction()
        {
            var transaction = _dbContext.Database.CurrentTransaction;

            if (transaction == null)
                throw new InvalidOperationException("Transaction must be explicitly started before Outbox AddAsync operation can be used");

            return transaction.GetDbTransaction();
        }

        private string GetMessageTypeName<T>(T data)
        {
            var type = typeof(T);

            if (type.IsGenericType)
                throw new InvalidOperationException("Data type couldn't be generic to store in outbox");

            if (type.IsAbstract)
                throw new InvalidOperationException("Data type couldn't be abstract to store in outbox");

            if (type.IsInterface)
                throw new InvalidOperationException("Data type couldn't be interface to store in outbox");

            return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        }
    }
}
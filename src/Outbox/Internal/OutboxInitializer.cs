using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using UnitOfWork;

namespace Outbox.Internal;

internal sealed class OutboxInitializer(IUnitOfWork unitOfWork) : IOutboxInitializer
{
    private const string CreateOutboxTableQueryName = "OutboxTable.sql";

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var query = SqlQueriesReader.ReadWithCache(CreateOutboxTableQueryName);

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        var rawTransaction = transaction.GetDbTransaction();
        var rawConnection = rawTransaction.Connection;

        try
        {
            await rawConnection.ExecuteAsync(query, transaction: rawTransaction);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
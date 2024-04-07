using Kafka;
using Microsoft.EntityFrameworkCore;
using Outbox;
using Serialization;
using UnitOfWork;
using WebApi;
using WebApi.Events;
using WebApi.MessageHandlers;
using WebApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInvoices();
builder.Services.AddOutbox();
builder.Services.AddKafka(InvoiceCreatedEvent.Topic);
builder.Services.HandleMessage<RootMessageHandler>();
builder.Services.AddUnitOfWork();
builder.Services.AddSerialization();
builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration["DbConnectionString"] ?? throw new ArgumentException("The database configuration string is missing"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.MigrateAsync(CancellationToken.None);
var outboxInitializer = scope.ServiceProvider.GetRequiredService<IOutboxInitializer>();
await outboxInitializer.InitializeAsync(CancellationToken.None);
var kafkaStructureInitializer = scope.ServiceProvider.GetRequiredService<IKafkaStructureInitializer>();
await kafkaStructureInitializer.InitializeAsync(CancellationToken.None);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
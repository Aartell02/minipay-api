using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Events;

namespace MiniPay.Infrastructure.EventStore
{
    public class EventStore(EventStoreDbContext dbContext) : IEventStore
    {
        public async Task SaveAsync(IEnumerable<TransactionEvent> events)
        {
            var eventEntities = events.Select(e => new EventEntity
            {
                TransactionId = e.TransactionId,
                EventType = e.GetType().Name,
                Payload = Serialize(e),
                OccuredAt = DateTimeOffset.UtcNow
            });
            await dbContext.Events.AddRangeAsync(eventEntities);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<TransactionEvent>> LoadAsync(Guid transactionId)
        {
            var eventEntities = await dbContext.Events
                .Where(e => e.TransactionId == transactionId)
                .OrderBy(e => e.OccuredAt)
                .ToListAsync();

            return eventEntities.Select(Deserialize);
        }

        private static string Serialize(TransactionEvent @event) => @event switch
        {
            TransactionInitiated e => JsonSerializer.Serialize(e),
            PaymentAuthorized e => JsonSerializer.Serialize(e),
            FundsSettled e => JsonSerializer.Serialize(e),
            RefundRequested e => JsonSerializer.Serialize(e),
            _ => throw new InvalidOperationException($"Unknown event type: {@event.GetType().Name}")
        };

        private static TransactionEvent Deserialize(EventEntity entity) => entity.EventType switch
            {
                nameof(TransactionInitiated) => JsonSerializer.Deserialize<TransactionInitiated>(entity.Payload)!,
                nameof(PaymentAuthorized) => JsonSerializer.Deserialize<PaymentAuthorized>(entity.Payload)!,
                nameof(FundsSettled) => JsonSerializer.Deserialize<FundsSettled>(entity.Payload)!,
                nameof(RefundRequested) => JsonSerializer.Deserialize<RefundRequested>(entity.Payload)!,
                _ => throw new InvalidOperationException($"Unknown event type: {entity.EventType}")
            };
        
    }
}

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
            TransactionInitiatedEvent e => JsonSerializer.Serialize(e),
            TransactionAuthorizedEvent e => JsonSerializer.Serialize(e),
            TransactionSettledEvent e => JsonSerializer.Serialize(e),
            TransactionRefundRequestedEvent e => JsonSerializer.Serialize(e),
            _ => throw new InvalidOperationException($"Unknown event type: {@event.GetType().Name}")
        };

        private static TransactionEvent Deserialize(EventEntity entity) => entity.EventType switch
            {
                nameof(TransactionInitiatedEvent) => JsonSerializer.Deserialize<TransactionInitiatedEvent>(entity.Payload)!,
                nameof(TransactionAuthorizedEvent) => JsonSerializer.Deserialize<TransactionAuthorizedEvent>(entity.Payload)!,
                nameof(TransactionSettledEvent) => JsonSerializer.Deserialize<TransactionSettledEvent>(entity.Payload)!,
                nameof(TransactionRefundRequestedEvent) => JsonSerializer.Deserialize<TransactionRefundRequestedEvent>(entity.Payload)!,
                _ => throw new InvalidOperationException($"Unknown event type: {entity.EventType}")
            };
        
    }
}

using MiniPay.API.Domain.Transactions;

namespace MiniPay.API.Infrastructure.EventStore
{
    public interface IEventStore
    {
        Task SaveAsync(IEnumerable<TransactionEvent> events);
        Task<IEnumerable<TransactionEvent>> LoadAsync(Guid transactionId);
    }
}

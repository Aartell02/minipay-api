using MiniPay.Domain.Events;

namespace MiniPay.Application.Interfaces
{
    public interface IEventStore
    {
        Task SaveAsync(IEnumerable<TransactionEvent> events);
        Task<IEnumerable<TransactionEvent>> LoadAsync(Guid transactionId);
    }
}

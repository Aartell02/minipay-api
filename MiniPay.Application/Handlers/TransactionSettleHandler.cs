using MediatR;
using MiniPay.Application.Commands;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class TransactionSettleHandler(IEventStore eventStore) : IRequestHandler<TransactionSettleCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(TransactionSettleCommand request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");

            var transaction = Transaction.Rewind(events);
            transaction.Settle();
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }
}

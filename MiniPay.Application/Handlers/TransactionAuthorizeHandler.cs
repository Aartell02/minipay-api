using MediatR;
using MiniPay.Application.Commands;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class TransactionAuthorizeHandler(IEventStore eventStore) : IRequestHandler<TransactionAuthorizeCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(TransactionAuthorizeCommand request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");

            var transaction = Transaction.Rewind(events);
            transaction.Authorize();
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }
}

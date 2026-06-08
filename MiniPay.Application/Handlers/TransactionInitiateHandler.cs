using MediatR;
using MiniPay.Application.Commands;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class TransactionInitiateHandler(IEventStore eventStore) : IRequestHandler<TransactionInitiateCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(TransactionInitiateCommand request, CancellationToken cancellationToken)
        {
            var transaction = Transaction.Initiate(request.Amount, request.Currency);
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }
}

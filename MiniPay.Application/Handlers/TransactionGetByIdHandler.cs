using MediatR;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;
using MiniPay.Application.Queries;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class TransactionGetByIdHandler(IEventStore eventStore) : IRequestHandler<TransactionGetByIdQuery, TransactionDto>
    {
        public async Task<TransactionDto> Handle(TransactionGetByIdQuery request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");
            
            var transaction = Transaction.Rewind(events);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }
    
}

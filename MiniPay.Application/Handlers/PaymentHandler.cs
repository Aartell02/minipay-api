using MediatR;
using MiniPay.Application.Commands;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class InitiatePaymentHandler(IEventStore eventStore) : IRequestHandler<InitiatePaymentCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
        {
            var transaction = Transaction.Initiate(request.Amount, request.Currency);
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }

    public class AuthorizePaymentHandler(IEventStore eventStore) : IRequestHandler<AuthorizePaymentCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(AuthorizePaymentCommand request, CancellationToken cancellationToken)
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

    public class SettlePaymentHandler(IEventStore eventStore) : IRequestHandler<SettlePaymentCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(SettlePaymentCommand request, CancellationToken cancellationToken)
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
    
    public class GetPaymentByIdHandler(IEventStore eventStore) : IRequestHandler<GetPaymentByIdCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(GetPaymentByIdCommand request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");
            
            var transaction = Transaction.Rewind(events);
            return new TransactionDto(transaction.Id, transaction.Amount, transaction.Currency, transaction.Status.ToString(), []);
        }
    }
    
}

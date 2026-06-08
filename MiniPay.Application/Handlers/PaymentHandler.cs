using MediatR;
using MiniPay.Application.Commands;
using MiniPay.Application.Interfaces;
using MiniPay.Domain.Aggregates;

namespace MiniPay.Application.Handlers
{
    public class InitiatePaymentHandler(IEventStore eventStore) : IRequestHandler<InitiatePaymentCommand, Guid>
    {
        public async Task<Guid> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
        {
            var transaction = Transaction.Initiate(request.Amount, request.Currency);
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return transaction.Id;
        }
    }

    public class AuthorizePaymentHandler(IEventStore eventStore) : IRequestHandler<AuthorizePaymentCommand, Guid>
    {
        public async Task<Guid> Handle(AuthorizePaymentCommand request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");

            var transaction = Transaction.Rewind(events);
            transaction.Authorize();
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return transaction.Id;
        }
    }

    public class SettlePaymentHandler(IEventStore eventStore) : IRequestHandler<SettlePaymentCommand, Guid>
    {
        public async Task<Guid> Handle(SettlePaymentCommand request, CancellationToken cancellationToken)
        {
            var events = await eventStore.LoadAsync(request.Id);
            if (!events.Any())
                throw new InvalidOperationException($"Transaction {request.Id} not found.");

            var transaction = Transaction.Rewind(events);
            transaction.Settle();
            await eventStore.SaveAsync(transaction.UncommittedEvents);
            return transaction.Id;
        }
    }
    /*
    public class GetPaymentByIdHandler(IEventStore eventStore) : IRequestHandler<GetPaymentByIdCommand, Guid>
    {
        public async Task<Guid> Handle(GetPaymentByIdCommand request, CancellationToken cancellationToken)
        {

        }
    }
    */
}

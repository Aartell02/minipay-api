using MiniPay.API.Application.Enums;
using MiniPay.API.Domain.Events;

namespace MiniPay.API.Domain.Aggregates
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "";
        public TransactionStatus Status { get; private set; }

        private readonly List<TransactionEvent> _uncommittedEvents = new();
        public IReadOnlyList<TransactionEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

        public static Transaction Initiate(decimal amount, string currency)
        {
            var transaction = new Transaction();
            transaction.Apply(new TransactionInitiated(Guid.NewGuid(), DateTimeOffset.UtcNow, amount, currency));
            return transaction;
        }

        public void Authorize()
        {
            if (Status != TransactionStatus.Initiated)
                throw new InvalidOperationException("Only initiated transactions can be authorized.");
            Apply(new PaymentAuthorized(Id, DateTimeOffset.UtcNow));
        }

        private void Apply(TransactionEvent evt)
        {
            switch(evt)
            {
                case TransactionInitiated e:
                    Id = e.TransactionId;
                    Amount = e.Amount;
                    Currency = e.Currency;
                    Status = TransactionStatus.Initiated;
                    break;
                case PaymentAuthorized e:
                    Status = TransactionStatus.Authorized;
                    break;
                case FundsSettled e:
                    Status = TransactionStatus.Settled;
                    break;
                case RefundRequested e:
                    Status = TransactionStatus.Refunded;
                    break;
            }
            _uncommittedEvents.Add(evt);
        }
    }

}

using MiniPay.Domain.Enums;
using MiniPay.Domain.Events;

namespace MiniPay.Domain.Aggregates
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
            var transactionEvent = new TransactionInitiatedEvent(Guid.NewGuid(), amount, currency);
            transaction.Apply(transactionEvent);
            transaction._uncommittedEvents.Add(transactionEvent);
            return transaction;
        }

        public static Transaction Rewind(IEnumerable<TransactionEvent> events)
        {
            var transaction = new Transaction();
            foreach (var e in events)
                transaction.Apply(e);
            return transaction;
        }

        private void Apply(TransactionEvent evt)
        {
            switch(evt)
            {
                case TransactionInitiatedEvent e:
                    Id = e.TransactionId;
                    Amount = e.Amount;
                    Currency = e.Currency;
                    Status = TransactionStatus.Initiated;
                    break;
                case TransactionAuthorizedEvent e:
                    Status = TransactionStatus.Authorized;
                    break;
                case TransactionSettledEvent e:
                    Status = TransactionStatus.Settled;
                    break;
                case TransactionRefundRequestedEvent e:
                    Status = TransactionStatus.Refunded;
                    break;
            }
        }

        public void Authorize()
        {
            if (Status != TransactionStatus.Initiated)
                throw new InvalidOperationException("Only initiated transactions can be authorized.");

            var paymentAuthorizedEvent = new TransactionAuthorizedEvent(Id);
            Apply(paymentAuthorizedEvent);
            _uncommittedEvents.Add(paymentAuthorizedEvent);
        }


        public void Settle()
        {
            if (Status != TransactionStatus.Authorized)
                throw new InvalidOperationException("Only authorized transactions can be settled.");

            var fundsSettledEvent = new TransactionSettledEvent(Id);
            Apply(fundsSettledEvent);
            _uncommittedEvents.Add(fundsSettledEvent);
        }
    }
}

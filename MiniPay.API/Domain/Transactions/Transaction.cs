using MiniPay.API.Domain.Transactions;

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
            transaction.Apply(new TransactionInitiated(amount, currency));
            return transaction;
        }

        public void Authorize()
        {
            if (Status != TransactionStatus.Initiated)
                throw new InvalidOperationException("Only initiated transactions can be authorized.");
            Apply(new PaymentAuthorized());
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

        public static Transaction Replay(IEnumerable<TransactionEvent> events)
        {
            var transaction = new Transaction();
            foreach (var e in events)
                transaction.Apply(e);
            return transaction;
        }

        public void Settle()
        {
            if (Status != TransactionStatus.Authorized)
                throw new InvalidOperationException("Tylko authorized można rozliczyć.");
            Apply(new FundsSettled());
        }
    }

}

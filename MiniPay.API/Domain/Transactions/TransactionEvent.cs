namespace MiniPay.API.Domain.Transactions
{
    public abstract record TransactionEvent
    {
        public Guid TransactionId { get; init; } = Guid.NewGuid();
        public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    }
    public record TransactionInitiated(decimal Amount, string Currency) : TransactionEvent;
    public record PaymentAuthorized() : TransactionEvent;
    public record FundsSettled() : TransactionEvent;
    public record RefundRequested(string Reason) : TransactionEvent;
}

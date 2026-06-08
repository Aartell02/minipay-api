namespace MiniPay.Domain.Events
{
    public abstract record TransactionEvent(Guid TransactionId)
    {
        public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    }

    public record TransactionInitiated(Guid TransactionId, decimal Amount, string Currency) : TransactionEvent(TransactionId);

    public record PaymentAuthorized(Guid TransactionId) : TransactionEvent(TransactionId);

    public record FundsSettled(Guid TransactionId) : TransactionEvent(TransactionId);

    public record RefundRequested(Guid TransactionId, string Reason) : TransactionEvent(TransactionId);
}

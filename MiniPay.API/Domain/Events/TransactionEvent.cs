namespace MiniPay.API.Domain.Events
{
    public abstract record TransactionEvent(Guid TransactionId, DateTimeOffset OccurredAt);
    public record TransactionInitiated(Guid TransactionId, DateTimeOffset OccurredAt, decimal Amount, string Currency) : TransactionEvent(TransactionId, OccurredAt);
    public record PaymentAuthorized(Guid TransactionId, DateTimeOffset OccurredAt) : TransactionEvent(TransactionId, OccurredAt);
    public record FundsSettled(Guid TransactionId, DateTimeOffset OccurredAt) : TransactionEvent(TransactionId, OccurredAt);
    public record RefundRequested(Guid TransactionId, DateTimeOffset OccurredAt, string Reason) : TransactionEvent(TransactionId, OccurredAt);
}

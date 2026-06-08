namespace MiniPay.Domain.Events
{
    public record TransactionInitiatedEvent(Guid TransactionId, decimal Amount, string Currency) : TransactionEvent(TransactionId);
}

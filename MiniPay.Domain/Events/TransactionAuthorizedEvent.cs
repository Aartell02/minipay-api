namespace MiniPay.Domain.Events
{
    public record TransactionAuthorizedEvent(Guid TransactionId) : TransactionEvent(TransactionId);
}

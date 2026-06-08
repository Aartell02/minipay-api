namespace MiniPay.Domain.Events
{
    public record TransactionSettledEvent(Guid TransactionId) : TransactionEvent(TransactionId);
}

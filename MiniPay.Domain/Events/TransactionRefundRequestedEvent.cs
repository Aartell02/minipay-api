namespace MiniPay.Domain.Events
{
    public record TransactionRefundRequestedEvent(Guid TransactionId, string Reason) : TransactionEvent(TransactionId);
}

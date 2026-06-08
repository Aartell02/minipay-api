namespace MiniPay.Domain.Events
{
    public abstract record TransactionEvent(Guid TransactionId)
    {
        public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    }


}

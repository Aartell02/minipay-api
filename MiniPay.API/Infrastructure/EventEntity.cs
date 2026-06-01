namespace MiniPay.API.Infrastructure
{
    public class EventEntity
    {
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public string EventType { get; set; } = "";
        public string Payload { get; set; } = "";
        public DateTimeOffset OccuredAt { get; set; }
    }
}

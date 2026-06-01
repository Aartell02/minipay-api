namespace MiniPay.API.Domain.Transactions
{
    public enum TransactionStatus
    {
        Initiated,
        Authorized,
        Settled,
        Refunded
    }
}

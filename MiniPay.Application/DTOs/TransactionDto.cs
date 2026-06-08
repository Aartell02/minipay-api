namespace MiniPay.Application.DTOs
{
    public record TransactionDto(
        Guid Id,
        decimal Amount,
        string Currency,
        string Status,
        IEnumerable<string> History);
}

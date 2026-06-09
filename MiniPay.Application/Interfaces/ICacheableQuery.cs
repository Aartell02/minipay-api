namespace MiniPay.Application.Interfaces
{
    public interface ICacheableQuery
    {
        bool BypassCache { get; }
        string CacheKey { get; }
    }
}

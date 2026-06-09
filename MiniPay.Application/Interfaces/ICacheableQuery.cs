namespace MiniPay.Application.Interfaces
{
    internal interface ICacheableQuery
    {
        bool BypassCache { get; }
        string CacheKey { get; }
    }
}

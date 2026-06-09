using MediatR;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;

namespace MiniPay.Application.Queries
{
    public record TransactionGetByIdQuery(Guid Id) : IRequest<TransactionDto>, ICacheableQuery
    {
        public bool BypassCache => false;
        public string CacheKey => $"transaction_{Id}";
    };
}

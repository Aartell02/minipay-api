using MediatR;
using MiniPay.Application.DTOs;
using MiniPay.Application.Interfaces;

namespace MiniPay.Application.Commands
{
    public record TransactionSettleCommand(Guid Id) : IRequest<TransactionDto>, ICacheInvalidation
    {
        public string CacheKey => $"transaction_{Id}";
    };
}

using MediatR;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Commands
{
    public record TransactionAuthorizeCommand(Guid Id) : IRequest<TransactionDto>;
}

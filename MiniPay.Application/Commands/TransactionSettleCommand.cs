using MediatR;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Commands
{
    public record TransactionSettleCommand(Guid Id) : IRequest<TransactionDto>;
}

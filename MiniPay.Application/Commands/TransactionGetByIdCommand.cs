using MediatR;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Commands
{
    public record TransactionGetByIdCommand(Guid Id) : IRequest<TransactionDto>;
}

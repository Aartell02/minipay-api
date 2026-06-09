using MediatR;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Commands
{
    public record TransactionInitiateCommand(decimal Amount, string Currency) : IRequest<TransactionDto>
    {

    };
}

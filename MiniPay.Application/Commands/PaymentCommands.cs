using MediatR;
using MiniPay.Application.DTOs;

namespace MiniPay.Application.Commands
{
    public record InitiatePaymentCommand(decimal Amount, string Currency) : IRequest<TransactionDto>;
    public record AuthorizePaymentCommand(Guid Id) : IRequest<TransactionDto>;
    public record SettlePaymentCommand(Guid Id) : IRequest<TransactionDto>;
    public record GetPaymentByIdCommand(Guid Id) : IRequest<TransactionDto>;
}

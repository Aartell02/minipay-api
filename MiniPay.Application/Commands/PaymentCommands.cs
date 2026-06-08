using MediatR;

namespace MiniPay.Application.Commands
{
    public abstract record PaymentCommand() : IRequest<Guid>;
    public record InitiatePaymentCommand(decimal Amount, string Currency) : PaymentCommand();
    public record AuthorizePaymentCommand(Guid Id) : PaymentCommand();
    public record SettlePaymentCommand(Guid Id) : PaymentCommand();
    public record GetPaymentByIdCommand(Guid Id) : PaymentCommand();
}

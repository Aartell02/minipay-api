using MiniPay.Domain.Aggregates;
using MiniPay.Domain.Enums;

namespace MiniPay.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void Initiate_ShouldCreateTransactionWithCorrectAmount()
        {
            var transaction = Transaction.Initiate(100m, "PLN");

            Assert.Equal(100m, transaction.Amount);
            Assert.Equal(TransactionStatus.Initiated, transaction.Status);
            Assert.Single(transaction.UncommittedEvents);
        }

        [Fact]
        public void Authorize_ShouldChangeStatusToAuthorized()
        {
            var transaction = Transaction.Initiate(100m, "PLN");
            transaction.Authorize();

            Assert.Equal(TransactionStatus.Authorized, transaction.Status);
            Assert.Equal(2, transaction.UncommittedEvents.Count);
        }

        [Fact]
        public void Authorize_WhenAlreadyAuthorized_ShouldThrow()
        {
            var transaction = Transaction.Initiate(100m, "PLN");
            transaction.Authorize();

            Assert.Throws<InvalidOperationException>(() => transaction.Authorize());
        }

        [Fact]
        public void Settle_WhenNotAuthorized_ShouldThrow()
        {
            var transaction = Transaction.Initiate(100m, "PLN");
            Assert.Throws<InvalidOperationException>(() => transaction.Settle());
        }

        [Fact]
        public void Rewind_UncommitedEvents_ShouldBeEmpty()
        {
            var transaction = Transaction.Initiate(100m, "PLN");
            transaction.Authorize();
            var events = transaction.UncommittedEvents.ToList();

            var transactionRewind = Transaction.Rewind(events);
            Assert.Empty(transactionRewind.UncommittedEvents);
        }
    }
}

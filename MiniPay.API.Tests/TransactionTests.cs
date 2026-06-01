using MiniPay.API.Domain.Aggregates;
using MiniPay.API.Domain.Transactions;

namespace MiniPay.API.Tests
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
    }
}

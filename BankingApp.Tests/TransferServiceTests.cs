using Xunit;
using BankingApp;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BankingApp.Main.DbContext;
using BankingApp.Main.Services;
using static BankingApp.Main.Models.Models;

namespace BankingApp.Tests
{

    public class TransferServiceTests
    {
        [Fact]
        public async Task Transfer_Should_Update_Balance_When_Success()
        {
            // Arrange: buat in-memory DB
            var options = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("BankingTestDB")
                .Options;

            using var context = new BankingContext(options);
            context.Accounts.Add(new Account { Id = 1, OwnerName = "Rayhan", Balance = 5000 });
            context.Accounts.Add(new Account { Id = 2, OwnerName = "Budi", Balance = 1000 });
            context.SaveChanges();

            var service = new TransferService(context);

            // Act: lakukan transfer
            bool result = await service.TransferAsync(1, 2, 2000);

            // Assert: cek hasil
            Assert.True(result);
            Assert.Equal(3000, context.Accounts.Find(1).Balance); // saldo Rayhan berkurang
            Assert.Equal(3000, context.Accounts.Find(2).Balance); // saldo Budi bertambah
        }

        [Fact]
        public async Task Transfer_Should_Fail_When_Insufficient_Balance()
        {
            var options = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("BankingTestDB2")
                .Options;

            using var context = new BankingContext(options);
            context.Accounts.Add(new Account { Id = 1, OwnerName = "Rayhan", Balance = 1000 });
            context.Accounts.Add(new Account { Id = 2, OwnerName = "Budi", Balance = 2000 });
            context.SaveChanges();

            var service = new TransferService(context);

            bool result = await service.TransferAsync(1, 2, 5000);

            Assert.False(result); // gagal karena saldo tidak cukup
        }

        [Fact]
        public async Task Transfer_Should_Insert_TransactionRecord()
        {
            var options = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("BankingTestDB3")
                .Options;

            using var context = new BankingContext(options);
            context.Accounts.Add(new Account { Id = 1, OwnerName = "Rayhan", Balance = 5000 });
            context.Accounts.Add(new Account { Id = 2, OwnerName = "Budi", Balance = 1000 });
            context.SaveChanges();

            var service = new TransferService(context);

            bool result = await service.TransferAsync(1, 2, 2000);

            var trx = context.Transactions.FirstOrDefault();

            Assert.True(result);
            Assert.NotNull(trx);
            Assert.Equal(2000, trx.Amount);
            Assert.Equal(1, trx.FromAccountId);
            Assert.Equal(2, trx.ToAccountId);
        }

    }

}

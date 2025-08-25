using BankingApp.Main.DbContext;
using static BankingApp.Main.Models.Models;

namespace BankingApp.Main.Services
{
    public class TransferService
    {
        private readonly BankingContext _context;

        public TransferService(BankingContext context)
        {
            _context = context;
        }

        public async Task<bool> TransferAsync(int fromId, int toId, decimal amount)
        {
            var from = await _context.Accounts.FindAsync(fromId);
            var to = await _context.Accounts.FindAsync(toId);

            if (from == null || to == null) return false;
            if (from.Balance < amount) return false;

            from.Balance -= amount;
            to.Balance += amount;

            _context.Transactions.Add(new Transaction
            {
                FromAccountId = fromId,
                ToAccountId = toId,
                Amount = amount
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }

}

using Microsoft.EntityFrameworkCore; // using directive yang BENAR
using static BankingApp.Main.Models.Models; // Asumsikan kelas Account dan Transaction ada di namespace ini

namespace BankingApp.Main.DbContext
{
    public class BankingContext : Microsoft.EntityFrameworkCore.DbContext // Sekarang 'DbContext' dikenali sebagai class
    {
        // Konstruktor
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
        }

        // DbSet Properties
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
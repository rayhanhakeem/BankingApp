namespace BankingApp.Main.Models
{
    public class Models
    {
        public class Account
        {
            public int Id { get; set; }
            public string OwnerName { get; set; } = string.Empty;
            public decimal Balance { get; set; }
            public ICollection<Transaction> Transactions { get; set; }
        }

        public class Transaction
        {
            public int Id { get; set; }
            public int FromAccountId { get; set; }
            public int ToAccountId { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }

    }
}

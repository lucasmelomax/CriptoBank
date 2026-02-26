

namespace CriptoBank.Domain.Models
{
    public class Portfolio
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid UserId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;


        public User User { get; private set; } = null!;
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public ICollection<Holding> Holdings { get; private set; } = new List<Holding>();

        protected Portfolio() { }

        public Portfolio(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}

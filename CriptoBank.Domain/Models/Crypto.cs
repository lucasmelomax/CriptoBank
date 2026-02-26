

namespace CriptoBank.Domain.Models
{
    public class Crypto
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Symbol { get; private set; } = string.Empty; 

        public string Name { get; private set; } = string.Empty;

        public string ExternalId { get; private set; } = string.Empty; 

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public ICollection<Holding> Holdings { get; private set; } = new List<Holding>();

        protected Crypto() { }

        public Crypto(string symbol, string name, string externalId)
        {
            Symbol = symbol;
            Name = name;
            ExternalId = externalId;
        }
    }
}

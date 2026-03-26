
namespace CriptoBank.Domain.Models
{
    public class Wallet
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public decimal Balance { get; private set; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }

        protected Wallet() { } 

        public Wallet(Guid userId)
        {
            UserId = userId;
            Balance = 0;
            Id = Guid.NewGuid();
        }

        public void Deposit(decimal valor)
        {
            if(valor <= 0) throw new ArgumentException("Valor deve ser positivo");
            Balance += valor;
        }
        public void WithDraw(decimal valor)
        {
            if (valor > Balance) throw new ArgumentException("Saldo insuficiente");
            if (valor <= 0) throw new ArgumentException("Valor do saque deve ser maior que 0.");
            Balance -= valor;
        }
    }
}

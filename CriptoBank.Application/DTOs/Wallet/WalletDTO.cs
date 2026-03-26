
using CriptoBank.Domain.Enums;

namespace CriptoBank.Application.DTOs.Wallet
{
    public class WalletDTO
    {
        public string Email { get; set; }
        public decimal Saldo { get; set; }
        public TransactionType tipoTransacao { get; set; }
        public DateTime TransactionDate { get; private set; }

        public WalletDTO(string email, decimal saldo, TransactionType tipoTransacao, DateTime transactionDate)
        {
            Email = email;
            Saldo = saldo;
            this.tipoTransacao = tipoTransacao;
            TransactionDate = transactionDate;
        }
    }
}

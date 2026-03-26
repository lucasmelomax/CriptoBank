

namespace CriptoBank.Application.DTOs.Wallet
{
    public class WalletSaldoDTO
    {
        public string Email { get; set; }
        public decimal Saldo { get; set; }

        public WalletSaldoDTO(string email, decimal saldo)
        {
            Email = email;
            Saldo = saldo;
        }
    }
}

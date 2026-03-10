
using CriptoBank.Domain.Enums;

namespace CriptoBank.Application.DTOs.Transaction
{
    public class TransactionDTO
    {
        public string Crypto { get; set; }

        public string Type { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalValue { get; set; }

        public string TransactionDate { get; set; }

    }
}

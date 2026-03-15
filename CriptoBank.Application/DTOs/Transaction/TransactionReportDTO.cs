

namespace CriptoBank.Application.DTOs.Transaction
{
    public record TransactionReportDTO
        (
            DateTime Date,
            string Type,
            string CryptoName,
            decimal Quantity,
            decimal TotalValue
        )
    {
    }
}

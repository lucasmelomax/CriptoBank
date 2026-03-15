

using CriptoBank.Application.DTOs.Transaction;

namespace CriptoBank.Application.Interfaces.ReportService
{
    public interface IReportService
    {
        byte[] GenerateTransactionReport(string userName, List<TransactionReportDTO> transactions);
    }
}

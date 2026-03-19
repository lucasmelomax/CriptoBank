

namespace CriptoBank.Application.DTOs.Message
{
    public record EmailGenerateReportMessage
    {
        public Guid UserId { get; init; }
        public string UserEmail { get; init; }
    }
}



namespace CriptoBank.Application.DTOs.Message
{
    public record GenerateReportMessage
    {
        public Guid UserId { get; init; }
        public string UserEmail { get; init; }
    }
}

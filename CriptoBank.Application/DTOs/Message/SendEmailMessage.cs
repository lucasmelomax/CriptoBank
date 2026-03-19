
namespace CriptoBank.Application.DTOs.Message
{
    public record SendEmailMessage
    {
        public string ToEmail { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }
        public string AttachmentPath { get; init; }
        public byte[] AttachmentBytes { get; set; } 

    }
}

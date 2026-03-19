using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MassTransit;
using CriptoBank.Application.DTOs.Message;

public class SendEmailConsumer : IConsumer<SendEmailMessage>
{
    private readonly ILogger<SendEmailConsumer> _logger;

    public SendEmailConsumer(ILogger<SendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Iniciando envio de e-mail para: {To}", msg.ToEmail);

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("CriptoBank", "no-reply@criptobank.com"));
        email.To.Add(new MailboxAddress("", msg.ToEmail));
        email.Subject = msg.Subject;

        var builder = new BodyBuilder
        {
            TextBody = msg.Body
        };

        if (msg.AttachmentBytes != null && msg.AttachmentBytes.Length > 0)
        {
            builder.Attachments.Add(msg.AttachmentPath, msg.AttachmentBytes);
            _logger.LogInformation("Anexo adicionado via memória.");
        }
        else if (!string.IsNullOrEmpty(msg.AttachmentPath) && File.Exists(msg.AttachmentPath))
        {
            builder.Attachments.Add(msg.AttachmentPath);
            _logger.LogInformation("Anexo adicionado via disco: {Path}", msg.AttachmentPath);
        }
        else
        {
            _logger.LogWarning("Nenhum anexo encontrado (Memória ou Disco) para: {Path}", msg.AttachmentPath);
        }

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.CheckCertificateRevocation = false;

            await smtp.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, SecureSocketOptions.Auto);

            await smtp.AuthenticateAsync("bc124c354a0936", "2f775343a1641b");

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("[SUCESSO] Email enviado para {To}!", msg.ToEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail via MailKit");
            throw; 
        }
    }
}
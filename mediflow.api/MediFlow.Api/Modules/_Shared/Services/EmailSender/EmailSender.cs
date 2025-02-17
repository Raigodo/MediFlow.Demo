using System.Net;
using System.Net.Mail;

namespace MediFlow.Api.Modules._Shared.Services.EmailSender;

public sealed class EmailSender(IConfiguration configuration) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var mail = "mediflow.noreply@gmail.com";
        var password = configuration.GetSection("Integrations:Gmail:Password").Value;

        if (password is null)
        {
            throw new ArgumentNullException("Gmail password");
        }

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, password)
        };

        var mailMessage = new MailMessage(from: mail, to: email, subject, message);
        return client.SendMailAsync(mailMessage);
    }
}

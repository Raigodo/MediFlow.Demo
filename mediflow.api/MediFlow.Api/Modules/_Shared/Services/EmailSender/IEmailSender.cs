namespace MediFlow.Api.Modules._Shared.Services.EmailSender;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}

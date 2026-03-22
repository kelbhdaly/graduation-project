using System.Net.Mail;

namespace MedicalApp.BusinessLogic.EmailConflguration
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailMessage email);
    }
}

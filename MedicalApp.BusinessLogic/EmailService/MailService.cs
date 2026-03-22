using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
namespace MedicalApp.BusinessLogic.EmailConflguration
{
    public class MailService : IMailService
    {

        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailMessage email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_configuration["MailSettings:Email"]!),
                Subject = email.Subject
            };

            mail.From.Add(new MailboxAddress(
                _configuration["MailSettings:DisplayName"],
                _configuration["MailSettings:Email"]!
            ));

            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder
            {
                TextBody = email.Body
            };
            mail.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation = false;
            await smtp.ConnectAsync(
                _configuration["MailSettings:Host"],
                int.Parse(_configuration["MailSettings:Port"]!),
               SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(
                _configuration["MailSettings:Email"],
                _configuration["MailSettings:Password"]
            );
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }

    }
}

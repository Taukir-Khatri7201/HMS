using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using HMS.Utility;

namespace HMS.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            this.emailConfig = emailConfig;
        }
        public async Task SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            try
            {
                await Send(emailMessage);
            }
            catch
            {
                
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("HMS Team", emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };
            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using(var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            };
        }
    }
}

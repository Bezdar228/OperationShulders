using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace OperationShuldersAPi.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var host = _configuration["SmtpSettings:Host"];
            var port = int.Parse(_configuration["SmtpSettings:Port"]);
            var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"]);
            var useStartTls = bool.Parse(_configuration["SmtpSettings:UseStartTls"]);
            var username = _configuration["SmtpSettings:Username"];
            var password = _configuration["SmtpSettings:Password"];

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(username, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // Включаем или отключаем SSL в зависимости от настроек
                client.EnableSsl = enableSsl;

                // Включаем STARTTLS (если необходимо)
                if (useStartTls)
                {
                    client.EnableSsl = true; // Иногда требуется для совместимости
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}

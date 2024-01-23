using Email.Application.Contracts;
using EventBus.Entities.Email;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Email.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration) =>
            _configuration = configuration;
        public void Send(EmailBuilder builder, EmailModelSignUp model)
        {
            string mail = new EmailCreator().CreateMail(model.Nickname, builder, model.Token);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration["Mail:From"]!);
            message.To.Add(model.Email);
            message.Subject = model.Subject;
            message.BodyEncoding = Encoding.UTF8;
            message.Body = mail;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = _configuration["Mail:Smtp:Client"]!;
            smtpClient.Port = int.Parse(_configuration["Mail:Smtp:Port"]!);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_configuration["Mail:Credentials:Client"],
                _configuration["Mail:Credentials:Password"]);

            smtpClient.Send(message);
        }
    }
}

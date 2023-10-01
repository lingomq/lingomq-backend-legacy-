using Email.BusinessLayer.Contracts;
using EventBus.Entities.Email;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Email.BusinessLayer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration) =>
            _configuration = configuration;
        public void Send(EmailBuilder builder, EmailModelSignUp model)
        {
            string mail = new EmailCreator().CreateMail(model.Nickname, builder, model.Token);
            MailAddress from = new MailAddress(_configuration["Mail:From"]!, _configuration["Mail:Corp"]);
            MailAddress to = new MailAddress(model.Email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = model.Subject;
            message.ReplyToList.Add(new MailAddress(_configuration["Mail:From"]!));
            message.BodyEncoding = Encoding.UTF8;
            message.Body = mail;
            message.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(_configuration["Mail:Smtp:Client"],
                int.Parse(_configuration["Mail:Smtp:Port"]!));
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(_configuration["Mail:Credentials:Client"],
                _configuration["Mail:Credentials:Password"]);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}

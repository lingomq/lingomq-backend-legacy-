using Email.BusinessLayer.Contracts;
using Email.BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Email.BusinessLayer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration) =>
            _configuration = configuration;
        public void Send(EmailBuilder builder, EmailModel model)
        {
            string mail = new EmailCreator().CreateMail(model.Nickname, builder, model.Token);
            MailAddress from = new MailAddress(_configuration["Mail:From"]!, _configuration["Mail:Corp"]);
            MailAddress to = new MailAddress(model.Email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = model.Subject;
            message.Body = mail;
            message.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(_configuration["Mail:Smtp:Client"],
                _configuration["Mail:Smtp:Password"]);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}

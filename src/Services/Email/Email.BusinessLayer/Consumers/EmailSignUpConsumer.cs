using Email.BusinessLayer.Contracts;
using Email.BusinessLayer.Models;
using Email.BusinessLayer.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Email.BusinessLayer.Consumers
{
    public class EmailSignUpConsumer : IConsumer<EmailModel>
    {
        private readonly ILogger<EmailSignUpConsumer> _logger;
        private readonly IEmailSender _sender;
        public EmailSignUpConsumer(ILogger<EmailSignUpConsumer> logger, IEmailSender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        public Task Consume(ConsumeContext<EmailModel> context)
        {
            EmailModel model = new EmailModel()
            {
                Nickname = context.Message.Nickname,
                Email = context.Message.Email,
                Token = context.Message.Token,
                Subject = context.Message.Subject
            };

            _sender.Send(new SignUpEmailBuilder(), model);

            _logger.LogInformation($"[+] Email succesfully sended to {context.Message.Email} " +
                $"(Nickname: {context.Message.Nickname})");

            return Task.CompletedTask;
        }
    }
}

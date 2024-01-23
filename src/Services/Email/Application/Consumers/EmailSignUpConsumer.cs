using Email.Application.Contracts;
using Email.Application.Services;
using EventBus.Entities.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Email.Application.Consumers
{
    public class EmailSignUpConsumer : IConsumer<EmailModelSignUp>
    {
        private readonly ILogger<EmailSignUpConsumer> _logger;
        private readonly IEmailSender _sender;
        public EmailSignUpConsumer(ILogger<EmailSignUpConsumer> logger, IEmailSender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        public Task Consume(ConsumeContext<EmailModelSignUp> context)
        {
            EmailModelSignUp model = new EmailModelSignUp()
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

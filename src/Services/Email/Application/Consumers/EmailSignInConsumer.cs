using Email.Application.Contracts;
using Email.Application.Services;
using EventBus.Entities.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Email.Application.Consumers
{
    public class EmailSignInConsumer : IConsumer<EmailModelSignIn>
    {
        private readonly ILogger<EmailSignInConsumer> _logger;
        private readonly IEmailSender _sender;
        public EmailSignInConsumer(ILogger<EmailSignInConsumer> logger, IEmailSender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        public Task Consume(ConsumeContext<EmailModelSignIn> context)
        {
            EmailModelSignUp model = new EmailModelSignUp()
            {
                Nickname = context.Message.Nickname,
                Email = context.Message.Email,
                Subject = context.Message.Subject
            };

            _sender.Send(new SignInEmailBuilder(), model);

            _logger.LogInformation($"[+] [SignIn] Sended to {context.Message.Email} " +
                $"(Nickname: {context.Message.Nickname});");

            return Task.CompletedTask;
        }
    }
}

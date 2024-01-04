using EventBus.Entities.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.EventBus.MassTransit.Consumers;
public class EmailConsumer : IConsumer<EmailModelSignUp>
{
    private readonly ILogger<EmailConsumer> _logger;
    public EmailConsumer(ILogger<EmailConsumer> logger) =>
        _logger = logger;
    public Task Consume(ConsumeContext<EmailModelSignUp> context)
    {
        _logger.LogInformation($"[+] Email succesfully sended to {context.Message.Email} " +
            $"(Nickname: {context.Message.Nickname})");

        return Task.CompletedTask;
    }
}

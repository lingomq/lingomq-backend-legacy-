using EventBus.Entities.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Authentication.BusinessLayer.MassTransit.Consumers
{
    public class EmailConsumer : IConsumer<EmailModel>
    {
        private readonly ILogger<EmailConsumer> _logger;
        public EmailConsumer(ILogger<EmailConsumer> logger) =>
            _logger = logger;
        public Task Consume(ConsumeContext<EmailModel> context)
        {
            _logger.LogInformation($"[+] Email succesfully sended to {context.Message.Email} " +
                $"(Nickname: {context.Message.Nickname})");

            return Task.CompletedTask;
        }
    }
}

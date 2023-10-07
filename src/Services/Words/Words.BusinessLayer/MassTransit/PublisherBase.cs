using MassTransit;
using Microsoft.Extensions.Logging;

namespace Words.BusinessLayer.MassTransit
{
    public class PublisherBase
    {
        private IBus _bus;
        private readonly ILogger<PublisherBase> _logger;
        public PublisherBase(IBus bus, ILogger<PublisherBase> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        public async Task Send<T>(T entity) where T : class
        {
            var sendEndpoint = await _bus.GetPublishSendEndpoint<T>();
            Type type = typeof(T);
            _logger.LogInformation($"[+] Successfully sended {type}");
            await sendEndpoint.Send(entity);
        }
    }
}

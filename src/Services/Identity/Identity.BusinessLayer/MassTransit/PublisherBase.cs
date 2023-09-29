using MassTransit;
using Microsoft.Extensions.Logging;

namespace Identity.BusinessLayer.MassTransit
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
            var sendPoint = await _bus.GetPublishSendEndpoint<T>();
            Type entityType = typeof(T);
            _logger.LogInformation("[+] [Identity Publisher] Send to {0}", entityType);
            await sendPoint.Send(entity);
        }
    }
}

using MassTransit;
using Microsoft.Extensions.Logging;

namespace Authentication.BusinessLayer.MassTransit
{
    public class Publisher
    {
        private IBus _bus;
        private readonly ILogger<Publisher> _logger;
        public Publisher(IBus bus, ILogger<Publisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        public async Task Send<T>(T entity) where T: class
        {
            var sendEndpoint = await _bus.GetPublishSendEndpoint<T>(); 
            Type type = typeof(T);
            _logger.LogInformation($"[+] Successfully sended {type}");
            await sendEndpoint.Send(entity);
        }
    }
}

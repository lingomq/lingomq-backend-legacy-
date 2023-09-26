using MassTransit;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Authentication.BusinessLayer.MassTransit
{
    public class Publisher
    {
        private IBus _bus;
        public Publisher(IBus bus) => _bus = bus;
        public async Task Send<T>(T entity) where T: class
        {
            var sendEndpoint = await _bus.GetPublishSendEndpoint<T>();
            await sendEndpoint.Send(entity);
        }
    }
}

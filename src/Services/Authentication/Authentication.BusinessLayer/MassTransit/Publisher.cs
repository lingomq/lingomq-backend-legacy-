using MassTransit;

namespace Authentication.BusinessLayer.MassTransit
{
    public class Publisher
    {
        private IBus _bus;
        public Publisher(IBus bus) => _bus = bus;
        public async Task Send<T>(T entity) where T: class =>
            await _bus.Send(entity);
    }
}

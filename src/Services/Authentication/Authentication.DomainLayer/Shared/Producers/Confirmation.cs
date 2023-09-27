namespace Authentication.DomainLayer.Shared.Producers
{
    public class Confirmation : MassTransitBase
    {
        public string? Token { get; set; }
    }
}

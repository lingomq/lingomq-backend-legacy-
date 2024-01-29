using System.Text.Json.Serialization;

namespace Finances.DomainLayer.Entities
{
    public class UserFinance : BaseEntity
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid FinanceId { get; set; }
        public Finance? Finance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EndSubscriptionDate { get; set; }
    }
}

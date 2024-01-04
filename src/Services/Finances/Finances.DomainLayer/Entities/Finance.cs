namespace Finances.DomainLayer.Entities
{
    public class Finance : BaseEntity
    {
        public string? FinanceName { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
    }
}

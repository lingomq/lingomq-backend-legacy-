using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface ILinkTypeRepository : IGenericRepository<LinkType, LinkType>
    {
        Task<LinkType?> GetByNameAsync(string name);
    }
}

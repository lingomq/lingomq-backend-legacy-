using DataAccess.Dapper.Contracts;
using Identity.Domain.Contracts;

namespace Identity.Application.Services.User;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublisherBase _publisher;
    public UserService(IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Entities.User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.Entities.User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCredentialsAsync(Domain.Entities.User user, UserCredentialsModel userCredentialsModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

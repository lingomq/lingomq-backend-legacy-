using DataAccess.Dapper.Contracts;
using EventBus.Entities.Identity.UserRoles;
using Identity.Application.EventBus.MassTransit;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions.ClientExceptions;

namespace Identity.Application.Services.UserRoleActions;
public class UserRoleService : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublisherBase _publisher;
    public UserRoleService(IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task CreateAsync(UserRole userRole, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRoles.AddAsync(userRole);

        await _publisher.Send(new IdentityModelUserRoleAdd()
        {
            Id = userRole.Id,
            Name = userRole.Name
        }, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        UserRole? role = await _unitOfWork.UserRoles.GetByIdAsync(id);
        if (role is null)
            throw new NotFoundException<UserRole>("role wasn't found");

        await _unitOfWork.UserRoles.DeleteAsync(id);

        await _publisher.Send(new IdentityModelUserRoleDelete()
        {
            Id = role.Id
        }, cancellationToken);
    }

    public async Task<UserRole> GetByIdAsync(Guid id)
    {
        UserRole? role = await _unitOfWork.UserRoles.GetByIdAsync(id);

        if (role is null)
            throw new NotFoundException<UserRole>();

        return role;
    }

    public async Task<List<UserRole>> GetRangeAsync(int count)
    {
        List<UserRole> roles = await _unitOfWork.UserRoles.GetAsync(count);
        return roles;
    }

    public async Task UpdateAsync(UserRole userRole, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRoles.UpdateAsync(userRole);

        await _publisher.Send(new IdentityModelUserRoleUpdate()
        {
            Id = userRole.Id,
            Name = userRole.Name
        });
    }
}

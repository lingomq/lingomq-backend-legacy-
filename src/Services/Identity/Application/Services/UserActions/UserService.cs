using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;
using Identity.DataAccess.Dapper.Contracts;
using EventBus.Entities.Identity.User;
using Identity.Application.EventBus.MassTransit;
using Identity.Domain.Contracts;
using Identity.Domain.Exceptions.ClientExceptions;

namespace Identity.Application.Services.UserActions;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublisherBase _publisher;
    public UserService(IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Domain.Entities.User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            throw new InvalidDataException<Domain.Entities.User>("Пользователя не существует");

        await _unitOfWork.Users.DeleteAsync(id);

        await _publisher.Send(new IdentityModelDeleteUser()
        {
            Id = id,
        });
    }

    public async Task<Domain.Entities.User> GetByIdAsync(Guid id)
    {
        Domain.Entities.User? user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
            throw new NotFoundException<Domain.Entities.User>("Пользователь не найден");

        return user;
    }

    public async Task UpdateAsync(Domain.Entities.User user, CancellationToken cancellationToken)
    {
        await _unitOfWork.Users.UpdateAsync(user);

        await _publisher.Send(new IdentityModelUpdateUser()
        {
            Id = user.Id,
            Email = user.Email,
            Phone = user.Phone
        }, cancellationToken);
    }

    public async Task UpdateCredentialsAsync(UserCredentialsModel userCredentialsModel, CancellationToken cancellationToken)
    {
        Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
        BaseKeyPair keyPair = cryptor
        .Crypt(userCredentialsModel.Password);

        Domain.Entities.User user = new Domain.Entities.User()
        {
            Id = userCredentialsModel.Id,
            PasswordHash = keyPair.Hash,
            PasswordSalt = keyPair.Salt
        };

        await _unitOfWork.Users.UpdateCredentialsAsync(user);

        await _publisher.Send(new IdentityModelUpdateUserCredentials()
        {
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt
        }, cancellationToken);
    }
}

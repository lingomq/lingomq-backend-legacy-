using DataAccess.Dapper.Contracts;
using Words.Domain.Contracts;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace Words.Application.Services.UserWordTypeActions;
public class UserWordTypeService : IUserWordTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserWordTypeService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task CreateAsync(UserWordType userWordType)
    {
        await _unitOfWork.UserWordTypes.AddAsync(userWordType);
    }

    public async Task DeleteAsync(Guid id)
    {
        UserWordType? userWordType = await _unitOfWork.UserWordTypes.GetByIdAsync(id);
        if (userWordType is null)
            throw new InvalidDataException<UserWordType>(parameters: new string[] { "id" });
        if (userWordType.UserWord!.User!.Id != id)
            throw new ForbiddenException<UserWordType>();

        await _unitOfWork.UserWordTypes.DeleteAsync(id);
    }

    public async Task<List<UserWordType>> GetByTypeIdAsync(Guid id)
    {
        List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetByTypeIdAsync(id);
        return userWordTypes;
    }

    public async Task<List<UserWordType>> GetByWordIdAsync(Guid id)
    {
        List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetByUserIdAsync(id);
        return userWordTypes;
    }

    public async Task<List<UserWordType>> GetRangeAsync(int range)
    {
        List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetAsync(range);
        return userWordTypes;
    }
}

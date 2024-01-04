using DataAccess.Dapper.Contracts;
using Words.Domain.Contracts;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace Words.Application.Services.WordTypeActions;
public class WordTypeService : IWordTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    public WordTypeService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task CreateAsync(WordType wordType)
    {
        await _unitOfWork.WordTypes.AddAsync(wordType);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.WordTypes.GetByIdAsync(id) is null)
            throw new InvalidDataException<WordType>(parameters: new string[] { "id" });

        await _unitOfWork.WordTypes.DeleteAsync(id);
    }

    public async Task<WordType> GetAsync(Guid id)
    {
        WordType? wordType = await _unitOfWork.WordTypes.GetByIdAsync(id);
        if (wordType is null)
            throw new NotFoundException<WordType>();

        return wordType;
    }

    public async Task<List<WordType>> GetRangeAsync(int range)
    {
        return await _unitOfWork.WordTypes.GetAsync(range);
    }

    public async Task UpdateAsync(WordType wordType)
    {
        if (await _unitOfWork.WordTypes.GetByIdAsync(wordType.Id) is null)
            throw new NotFoundException<WordType>();

        await _unitOfWork.WordTypes.UpdateAsync(wordType);
    }
}

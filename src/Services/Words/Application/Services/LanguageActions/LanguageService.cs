using DataAccess.Dapper.Contracts;
using EventBus.Entities.Words;
using Words.Application.EventBus.MassTransit;
using Words.Domain.Contracts;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace Words.Application.Services.LanguageActions;
public class LanguageService : ILanguageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublisherBase _publisher;
    public LanguageService(IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task CreateAsync(Language language)
    {
        await _unitOfWork.Languages.AddAsync(language);

        await _publisher.Send(new WordsLanguageCreate()
        {
            Id = language.Id,
            Name = language.Name
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.Languages.GetByIdAsync(id) is null)
            throw new NotFoundException<Language>();

        await _publisher.Send(new WordsLanguageDelete()
        {
            Id = id,
        });
        await _unitOfWork.Languages.DeleteAsync(id);
    }

    public async Task<List<Language>> GetAsync(int count)
    {
        List<Language> languages = await _unitOfWork.Languages.GetAsync(count);
        return languages;
    }

    public async Task<Language> GetAsync(Guid id)
    {
        Language? language = await _unitOfWork.Languages.GetByIdAsync(id);
        if (language is null)
            throw new NotFoundException<Language>();

        return language;
    }

    public async Task UpdateAsync(Language language)
    {
        if (await _unitOfWork.Languages.GetByIdAsync(language.Id) is null)
            throw new NotFoundException<Language>();

        await _unitOfWork.Languages.UpdateAsync(language);
        await _publisher.Send(new WordsLanguageUpdate()
        {
            Id = language.Id,
            Name = language.Name
        });
    }
}

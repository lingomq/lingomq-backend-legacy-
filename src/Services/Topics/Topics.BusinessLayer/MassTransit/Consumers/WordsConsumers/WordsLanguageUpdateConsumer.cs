using EventBus.Entities.Words;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.MassTransit.Consumers.WordsConsumers;

public class WordsLanguageUpdateConsumer : IConsumer<WordsLanguageUpdate>
{
    private readonly ILogger<WordsLanguageUpdateConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public WordsLanguageUpdateConsumer(ILogger<WordsLanguageUpdateConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<WordsLanguageUpdate> context)
    {
        Language language = new Language()
        {
            Id = context.Message.Id,
            Name = context.Message.Name
        };

        await _unitOfWork.Languages.UpdateAsync(language);

        _logger.LogInformation("[+] [Topics Update Consumer] Succesfully get message." +
                               "{0}:{1} has been updated", language.Id, language.Name);
    }
}
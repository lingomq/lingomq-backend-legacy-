using EventBus.Entities.Words;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Entities;

namespace Topics.Application.EventBus.MassTransit.Consumers.WordsConsumers;

public class WordsLanguageCreateConsumer : IConsumer<WordsLanguageCreate>
{
    private readonly ILogger<WordsLanguageCreateConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public WordsLanguageCreateConsumer(ILogger<WordsLanguageCreateConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<WordsLanguageCreate> context)
    {
        Language language = new Language()
        {
            Id = context.Message.Id,
            Name = context.Message.Name
        };

        await _unitOfWork.Languages.AddAsync(language);

        _logger.LogInformation("[+] [Topics Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", language.Id, language.Name);
    }
}
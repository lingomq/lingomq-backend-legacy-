using EventBus.Entities.Words;
using MassTransit;
using Microsoft.Extensions.Logging;
using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Entities;

namespace Topics.Application.EventBus.MassTransit.Consumers.WordsConsumers;

public class WordsLanguageDeleteConsumer : IConsumer<WordsLanguageDelete>
{
    private readonly ILogger<WordsLanguageDeleteConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public WordsLanguageDeleteConsumer(ILogger<WordsLanguageDeleteConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<WordsLanguageDelete> context)
    {
        Guid id = context.Message.Id;
        Language? language = await _unitOfWork.Languages.GetByIdAsync(id);

        if (language is null)
            _logger.LogError("[-] [Words LanguageDelete Consumer] " +
                             "Failed: Languages not found");

        await _unitOfWork.Languages.DeleteAsync(id);

        _logger.LogInformation("[+] [Words LanguageDelete Consumer] " +
                               "Success: Languages has been deleted");
    }
}
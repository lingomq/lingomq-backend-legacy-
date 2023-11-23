using EventBus.Entities.Identity.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Dtos;
namespace Words.BusinessLayer.MassTransit.Consumers;

public class WordsCreateUserConsumer : IConsumer<IdentityModelCreateUser>
{
    private readonly ILogger<WordsCreateUserConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public WordsCreateUserConsumer(ILogger<WordsCreateUserConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<IdentityModelCreateUser> context)
    {
        UserDto user = new UserDto()
        {
            Id = context.Message.UserId,
            Email = context.Message.Email,
            Phone = context.Message.Phone
        };

        await _unitOfWork.Users.AddAsync(user);
        _logger.LogInformation("[+] [Words Create Consumer] Succesfully get message." +
                               "{0}:{1} has been added", user.Id, user.Email);
    }
}
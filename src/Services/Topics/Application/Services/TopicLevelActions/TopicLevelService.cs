using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;
using Topics.Domain.Exceptions.ClientExceptions;

namespace Topics.Application.Services.TopicLevelActions;
public class TopicLevelService : ITopicLevelService
{
    private readonly IUnitOfWork _unitOfWork;
    public TopicLevelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateAsync(TopicLevel topicLevel)
    {
        await _unitOfWork.TopicLevels.AddAsync(topicLevel);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.TopicLevels.GetByIdAsync(id) is null)
            throw new NotFoundException<TopicLevel>();

        await _unitOfWork.TopicLevels.DeleteAsync(id);
    }

    public async Task<List<TopicLevel>> GetAsync(int count)
    {
        return await _unitOfWork.TopicLevels.GetAsync(count);
    }

    public async Task<TopicLevel> GetAsync(Guid id)
    {
        TopicLevel? level = await _unitOfWork.TopicLevels.GetByIdAsync(id);
        if (level is null)
            throw new NotFoundException<TopicLevel>();

        return level;
    }

    public async Task UpdateAsync(TopicLevel topicLevel)
    {
        if (await _unitOfWork.TopicLevels.GetByIdAsync(topicLevel.Id) is null)
            throw new NotFoundException<TopicLevel>();

        await _unitOfWork.TopicLevels.UpdateAsync(topicLevel);
    }
}

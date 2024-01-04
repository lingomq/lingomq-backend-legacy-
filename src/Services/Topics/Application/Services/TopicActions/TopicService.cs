using Topics.Domain.Contracts;
using Topics.Domain.Entities;
using Topics.Domain.Models;
using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Exceptions.ClientExceptions;

namespace Topics.Application.Services.TopicActions;
public class TopicService : ITopicService
{
    private readonly IUnitOfWork _unitOfWork;
    public TopicService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(Topic topic)
    {
        await _unitOfWork.Topics.AddAsync(topic);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.Topics.GetByIdAsync(id) is null)
            throw new InvalidDataException<Topic>(new string[] { "Id" });

        await _unitOfWork.Topics.DeleteAsync(id);
    }

    public async Task<List<Topic>> GetAsync(int skip, int take)
    {
        List<Topic> topics = await _unitOfWork.Topics.GetAsync(skip, take);
        return topics;
    }

    public async Task<Topic> GetAsync(Guid id)
    {
        Topic? topic = await _unitOfWork.Topics.GetByIdAsync(id);
        if (topic is null)
            throw new NotFoundException<Topic>();

        return topic;
    }

    public async Task UpdateAsync(Topic topic)
    {
        if (await _unitOfWork.Topics.GetByIdAsync(topic.Id) is null)
            throw new InvalidDataException<Topic>(new string[] { "Id" });

        await _unitOfWork.Topics.UpdateAsync(topic);
    }

    public async Task<List<Topic>> UseFilters(TopicFilters topicFilters)
    {
        List<Topic> topics = await _unitOfWork.Topics.GetByTopicFiltersAsync(topicFilters);
        return topics;
    }
}

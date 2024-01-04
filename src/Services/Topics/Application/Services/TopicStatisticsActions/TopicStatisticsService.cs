using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;
using Topics.Domain.Exceptions.ClientExceptions;

namespace Topics.Application.Services.TopicStatisticsActions;
public class TopicStatisticsService : ITopicStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    public TopicStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateAsync(TopicStatistics topicStatistics)
    {
        if (!await CheckStatisticsDoesNotExist(topicStatistics))
            throw new ConflictException<TopicStatistics>();

        await _unitOfWork.TopicStatistics.AddAsync(topicStatistics);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.TopicStatistics.GetByIdAsync(id) is null)
            throw new InvalidDataException<TopicStatistics>(new string[] { "Id" });

        await _unitOfWork.TopicStatistics.DeleteAsync(id);
    }

    public async Task<List<TopicStatistics>> GetAsync(int count)
    {
        return await _unitOfWork.TopicStatistics.GetAsync(count);
    }

    public async Task<TopicStatistics> GetAsync(Guid id)
    {
        TopicStatistics? statistics = await _unitOfWork.TopicStatistics.GetByIdAsync(id);
        if (statistics is null)
            throw new NotFoundException<TopicStatistics>();

        return statistics;
    }

    public async Task<List<TopicStatistics>> GetByTopicIdAsync(Guid id)
    {
        return await _unitOfWork.TopicStatistics.GetByTopicIdAsync(id);
    }

    public async Task UpdateAsync(TopicStatistics topicStatistics)
    {
        if (await _unitOfWork.TopicStatistics.GetByIdAsync(topicStatistics.Id) is null)
            throw new InvalidDataException<TopicStatistics>(new string[] { "Id" });

        await _unitOfWork.TopicStatistics.UpdateAsync(topicStatistics);
    }

    private async Task<bool> CheckStatisticsDoesNotExist(TopicStatistics statistics)
    {
        List<TopicStatistics> statisticsFromDb = await _unitOfWork.TopicStatistics.GetByTopicIdAsync(statistics.TopicId);
        if (statisticsFromDb.Count == 0 || !statisticsFromDb.Any())
            return true;

        if (statisticsFromDb.Where(x => x.StatisticsTypeId == statistics.StatisticsTypeId).Count() == 0)
            return true;

        return false;
    }
}

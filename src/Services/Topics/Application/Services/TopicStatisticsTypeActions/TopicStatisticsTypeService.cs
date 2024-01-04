using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Contracts;
using Topics.Domain.Entities;
using Topics.Domain.Exceptions.ClientExceptions;

namespace Topics.Application.Services.TopicStatisticsTypeActions;
public class TopicStatisticsTypeService : ITopicStatisticsTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    public TopicStatisticsTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(TopicStatisticsType topicStatisticsType)
    {
        await _unitOfWork.StatisticsTypes.AddAsync(topicStatisticsType);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (await _unitOfWork.StatisticsTypes.GetByIdAsync(id) is null)
            throw new InvalidDataException<TopicStatisticsType>(new string[] { "Id" });

        await _unitOfWork.StatisticsTypes.DeleteAsync(id);
    }

    public async Task<List<TopicStatisticsType>> GetAsync(int range = int.MaxValue)
    {
        return await _unitOfWork.StatisticsTypes.GetAsync(range);
    }

    public async Task UpdateAsync(TopicStatisticsType entity)
    {
        if (await _unitOfWork.StatisticsTypes.GetByIdAsync(entity.Id) is null)
            throw new InvalidDataException<TopicStatisticsType>(new string[] { "Id" });

        await _unitOfWork.StatisticsTypes.UpdateAsync(entity);
    }
}

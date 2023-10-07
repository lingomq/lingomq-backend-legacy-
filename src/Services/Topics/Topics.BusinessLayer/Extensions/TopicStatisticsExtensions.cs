using Topics.BusinessLayer.Dtos;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Extensions;

public static class TopicStatisticsExtensions
{
    public static TopicStatistics ToModel(this TopicStatisticsDto statistics)
    {
        return new TopicStatistics()
        {
            Id = statistics.Id,
            TopicId = statistics.TopicId,
            StatisticsTypeId = statistics.StatisticsTypeId,
            StatisticsDate = statistics.StatisticsDate,
            UserId = statistics.UserId
        };
    }
}
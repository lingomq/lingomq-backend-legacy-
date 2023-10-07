using Topics.BusinessLayer.Dtos;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Extensions;

public static class TopicExtensions
{
    public static TopicDto ToDto(this Topic topic)
    {
        return new TopicDto()
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            LanguageId = topic.LanguageId,
            TopicLevelId = topic.TopicLevelId,
            CreationalDate = topic.CreationalDate,
            Icon = topic.Icon
        };
    }
    public static Topic ToModel(this TopicDto topic)
    {
        return new Topic()
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            LanguageId = topic.LanguageId,
            TopicLevelId = topic.TopicLevelId,
            CreationalDate = topic.CreationalDate,
            Icon = topic.Icon
        };
    }
}
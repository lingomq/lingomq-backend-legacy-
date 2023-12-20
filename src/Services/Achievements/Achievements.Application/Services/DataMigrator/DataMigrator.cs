using Achievements.DataAccess.Dapper.Contracts;
using Achievements.Domain.Entities;

namespace Achievements.Application.Services.DataMigrator;
public class DataMigrator : IDataMigrator
{
    private readonly IUnitOfWork _unitOfWork;
    public DataMigrator(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task AddAchievementsAsync()
    {
        List<Achievement> achievements = new List<Achievement>()
            {
                new Achievement()
                {
                    Id = Guid.NewGuid(),
                    Name = "Начало начал",
                    Content = "Добавить первое слово",
                    ImageUri = "data:image/png;base64,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"
                }
            };

        var achievementsFromDatabase = await _unitOfWork.Achievements.GetAsync(10);

        foreach (var achievementName in achievementsFromDatabase.Select(x => x.Name))
        {
            if (achievements.Select(x => x.Name).Contains(achievementName!))
                achievements.Remove(achievements.FirstOrDefault(x => x.Name == achievementName)!);
        }

        foreach (Achievement achievement in achievements)
            await _unitOfWork.Achievements.AddAsync(achievement);
    }

    public async Task MigrateAsync()
    {
        await AddAchievementsAsync();
    }
}

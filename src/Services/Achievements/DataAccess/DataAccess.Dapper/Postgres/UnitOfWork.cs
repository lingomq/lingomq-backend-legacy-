using Achievements.DataAccess.Dapper.Contracts;

namespace Achievements.DataAccess.Dapper.Postgres;
public class UnitOfWork : IUnitOfWork
{
    public IAchievementRepository Achievements { get; }

    public IUserAchievementRepository UserAchievements { get; }

    public IUserRepository Users { get; }
    public UnitOfWork(IAchievementRepository achievementRepository,
        IUserAchievementRepository userAchievementRepository,
        IUserRepository userRepository)
    {
        Achievements = achievementRepository;
        UserAchievements = userAchievementRepository;
        Users = userRepository;
    }
}

namespace Achievements.DataAccess.Dapper.Contracts;
public interface IUnitOfWork
{
    IAchievementRepository Achievements { get; }
    IUserAchievementRepository UserAchievements { get; }
    IUserRepository Users { get; }
}

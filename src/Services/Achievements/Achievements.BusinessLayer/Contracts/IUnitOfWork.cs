namespace Achievements.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        IAchievementRepository Achievements { get; }
        IUserAchievementRepository UserAchievements { get; }
        IUserRepository Users { get; }
    }
}

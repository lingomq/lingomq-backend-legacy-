using Identity.DataAccess.Dapper.Contracts;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions.ClientExceptions;

namespace Identity.Application.Services.UserStatisticsActions;
public class UserStatisticsService : IUserStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserStatisticsService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    public async Task AddHourToStatisticsAsync(Guid id)
    {
        UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(id);

        if (statistics is null)
            throw new NotFoundException<UserStatistics>("The statistics was not found");

        statistics.TotalHours += 1;

        await _unitOfWork.UserStatistics.UpdateAsync(statistics);
    }

    public async Task AddVisitationToStatisticsAsync(Guid id)
    {
        if (await _unitOfWork.Users.GetByIdAsync(id) is null)
            throw new NotFoundException<User>();

        UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(id);
        if (statistics is null)
            throw new NotFoundException<UserStatistics>();

        int hourSubstraction = (DateTime.Now.ToUniversalTime() - statistics.LastUpdateAt).Hours;

        if (hourSubstraction > 24 && hourSubstraction <= 48 || statistics.VisitStreak == 0)
        {
            statistics.VisitStreak += 1;
            statistics.LastUpdateAt = DateTime.Now.ToUniversalTime();
        }
        else if (hourSubstraction > 48)
            statistics.VisitStreak = 0;


        await _unitOfWork.UserStatistics.UpdateAsync(statistics);
    }

    public async Task AddWordToStatisticsAsync(Guid id)
    {
        UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(id);
        UserInfo? userInfo = await _unitOfWork.UserInfos.GetByUserIdAsync(id);

        if (statistics is null)
            throw new NotFoundException<UserStatistics>("The statistics was not found");

        int daysFromRegister = (DateTime.Now - userInfo!.CreationalDate).Days;
        if (daysFromRegister == 0) daysFromRegister = 1;

        statistics.TotalWords += 1;
        statistics.AvgWords = statistics.TotalWords / daysFromRegister;

        await _unitOfWork.UserStatistics.UpdateAsync(statistics);
    }

    public async Task<UserStatistics> GetByIdAsync(Guid id)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException<User>("The user doesn't found");

        UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(id);

        if (statistics is null)
            throw new NotFoundException<UserStatistics>("The statistics was not found");

        return statistics;
    }
}

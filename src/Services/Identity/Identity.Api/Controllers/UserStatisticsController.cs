using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserStatisticsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        [HttpGet("{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user is null)
                throw new NotFoundException<User>("The user doesn't found");

            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(userId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
        [HttpPut("hour/add")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddHour()
        {
            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(UserId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            int hourSubstraction = (DateTime.Now - statistics.LastUpdateAt).Hours;

            if (hourSubstraction > 1)
            {
                statistics.TotalHours += 1;
                statistics.LastUpdateAt = DateTime.Now.ToUniversalTime();
            }

            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
        [HttpPut("word/add")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddWord()
        {
            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(UserId);
            UserInfo? userInfo = await _unitOfWork.UserInfos.GetByUserIdAsync(UserId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            int daysFromRegister = (DateTime.Now - userInfo!.CreationalDate).Days;

            statistics.TotalWords += 1;
            statistics.AvgWords = statistics.TotalWords / daysFromRegister;

            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
        [HttpPut("visit")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> UpdateVisitStreak()
        {
            if (await _unitOfWork.Users.GetByIdAsync(UserId) is null)
                throw new NotFoundException<User>();

            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(UserId);
            if (statistics is null)
                throw new NotFoundException<UserStatistics>();

            int hourSubstraction = (DateTime.Now - statistics.LastUpdateAt).Hours;

            if (hourSubstraction > 24 && hourSubstraction <= 48)
            {
                statistics.VisitStreak += 1;
                statistics.LastUpdateAt = DateTime.Now.ToUniversalTime();
            }
            else if (hourSubstraction > 48)
                statistics.VisitStreak = 0;


            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
    }
}

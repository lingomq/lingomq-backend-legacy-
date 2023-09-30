using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserStatisticsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

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
        [HttpPut("hour/add/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddHour(Guid userId)
        {
            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(userId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            statistics.TotalHours += 1;

            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
        [HttpPut("word/add/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddWord(Guid userId)
        {
            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(userId);
            UserInfo? userInfo = await _unitOfWork.UserInfos.GetByUserIdAsync(userId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            int daysFromRegister = (DateTime.Now - userInfo!.CreationalDate).Days;

            statistics.TotalWords += 1;
            statistics.AvgWords = statistics.TotalWords / daysFromRegister;

            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            return LingoMq.Responses.StatusCode.OkResult(statistics);
        }
    }
}

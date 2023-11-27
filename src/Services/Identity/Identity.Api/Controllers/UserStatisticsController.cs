﻿using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/identity/user/statistics")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public UserStatisticsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        [HttpGet("id/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user is null)
                throw new NotFoundException<User>("The user doesn't found");

            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(userId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            _logger.Info("GET /{userId} {0}", nameof(UserStatistics));
            return LingoMq.Responses.LingoMqResponse.OkResult(statistics);
        }
        [HttpPut("hour/add")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> AddHour()
        {
            UserStatistics? statistics = await _unitOfWork.UserStatistics.GetByUserIdAsync(UserId);

            if (statistics is null)
                throw new NotFoundException<UserStatistics>("The statistics was not found");

            statistics.TotalHours += 1;

            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            _logger.Info("PUT /hour/add {0}", nameof(UserStatistics));
            return LingoMq.Responses.LingoMqResponse.OkResult(statistics);
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

            _logger.Info("PUT /word/add {0}", nameof(UserStatistics));
            return LingoMq.Responses.LingoMqResponse.OkResult(statistics);
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

            if ((hourSubstraction > 24 && hourSubstraction <= 48) || statistics.VisitStreak == 0)
            {
                statistics.VisitStreak += 1;
                statistics.LastUpdateAt = DateTime.Now.ToUniversalTime();
            }
            else if (hourSubstraction > 48)
                statistics.VisitStreak = 0;


            await _unitOfWork.UserStatistics.UpdateAsync(statistics);

            _logger.Info("PUT /visit {0}", nameof(UserStatistics));
            return LingoMq.Responses.LingoMqResponse.OkResult(statistics);
        }
    }
}

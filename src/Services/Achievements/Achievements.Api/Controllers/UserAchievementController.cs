using Achievements.Api.Common;
using Achievements.BusinessLayer.Contracts;
using Achievements.BusinessLayer.Exceptions.ClientExceptions;
using Achievements.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;

namespace Achievements.Api.Controllers
{
    [Route("api/achievements/user")]
    [ApiController]
    public class UserAchievementController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public UserAchievementController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        [HttpGet]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get()
        {
            List<UserAchievement> achievements = await _unitOfWork.UserAchievements.GetByUserIdAsync(UserId);
            _logger.Info("GET / {0}", nameof(List<Achievement>));
            return LingoMqResponse.OkResult(achievements);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            List<UserAchievement> achievements = await _unitOfWork.UserAchievements.GetByUserIdAsync(userId);
            _logger.Info("GET /{userId} {0}", nameof(List<Achievement>));
            return LingoMqResponse.OkResult(achievements);
        }

        [HttpGet("count/{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetAchievemensCount(Guid userId)
        {
            int count = await _unitOfWork.UserAchievements.GetCountAchievementsByUserIdAsync(userId);
            _logger.Info("GET /count/{userId} Count");
            return LingoMqResponse.OkResult(new { Count = count });
        }

        [HttpGet("all/{count}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> GetAll(int count = int.MaxValue)
        {
            List<UserAchievement> userAchievements = await _unitOfWork.UserAchievements.GetAsync(count);
            _logger.Info("GET /all/{count} {0}", nameof(List<Achievement>));
            return LingoMqResponse.OkResult(userAchievements);
        }

        [HttpGet("id/{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> GetById(Guid id)
        {
            UserAchievement? userAchievement = await _unitOfWork.UserAchievements.GetByIdAsync(id);
            if (userAchievement is null)
                throw new NotFoundException<UserAchievement>();

            _logger.Info("GET /id/{id} {0}", nameof(Achievement));
            return LingoMqResponse.OkResult(userAchievement);
        }
    }
}

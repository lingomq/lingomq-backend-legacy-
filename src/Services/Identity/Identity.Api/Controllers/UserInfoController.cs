using EventBus.Entities.Identity.UserInfo;
using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.BusinessLayer.MassTransit;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/v0/identity/user/info")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublisherBase _publisher;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);
        public UserInfoController(IUnitOfWork unitOfWork, PublisherBase publisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpGet]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get()
        {
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(UserId);

            if (user is null)
                throw new NotFoundException<User>("Пользователь не найден");

            UserInfo? info = await _unitOfWork.UserInfos.GetByUserIdAsync(UserId);
            if (info is null)
                throw new NotFoundException<UserInfo>("Данные не найдены");

            _logger.Info("GET / {0}", nameof(UserDto));
            return LingoMq.Responses.LingoMqResponse.OkResult(info);
        }
        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetRange(int range = int.MaxValue)
        {
            List<UserInfo> infos = await _unitOfWork.UserInfos.GetAsync(range);

            _logger.Info("GET /all/{range} {0}", nameof(List<UserInfo>));
            return LingoMq.Responses.LingoMqResponse.OkResult(infos);
        }
        [HttpGet("{nickname}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(string nickname)
        {
            UserInfo? info = await _unitOfWork.UserInfos.GetByNicknameAsync(nickname);

            if (info is null)
                throw new NotFoundException<UserInfo>("Данные не найдены");

            _logger.Info("GET /{nickname} {0}", nameof(UserInfo));
            return LingoMq.Responses.LingoMqResponse.OkResult(info);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Update(UserInfo info)
        {
            if (!info.UserId.Equals(UserId))
                throw new ForbiddenException<UserInfo>("Вы не авторизованы");

            await _unitOfWork.UserInfos.UpdateAsync(info);

            await _publisher.Send(new IdentityModelUpdateUserInfo()
            {
                Id = info.Id,
                Nickname = info.Nickname,
                Additional = info.Additional,
                ImageUri = info.ImageUri,
                RoleId = info.RoleId,
                UserId = info.UserId,
                CreationalDate = info.CreationalDate,
                IsRemoved = info.IsRemoved
            });

            _logger.Info("PUT / {0}", nameof(UserInfo));
            return LingoMq.Responses.LingoMqResponse.OkResult(info, "succesfully updated");
        }
        [HttpPut("admin")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> UpdateByAdmin(UserInfo info)
        {
            if (await _unitOfWork.UserInfos.GetByIdAsync(info.Id) is null)
                throw new NotFoundException<UserInfo>("Данные не найдены");

            if (await _unitOfWork.UserInfos.GetByUserIdAsync(info.UserId) is null)
                throw new NotFoundException<User>("Пользователь не найден");

            await _unitOfWork.UserInfos.UpdateAsync(info);

            await _publisher.Send(new IdentityModelUpdateUserInfo()
            {
                Id = info.Id,
                Nickname = info.Nickname,
                Additional = info.Additional,
                ImageUri = info.ImageUri,
                RoleId = info.RoleId,
                UserId = info.UserId,
                CreationalDate = info.CreationalDate,
                IsRemoved = info.IsRemoved
            });

            _logger.Info("PUT /admin {0}", nameof(UserInfo));
            return LingoMq.Responses.LingoMqResponse.OkResult(info, "succesfully updated");
        }
    }
}

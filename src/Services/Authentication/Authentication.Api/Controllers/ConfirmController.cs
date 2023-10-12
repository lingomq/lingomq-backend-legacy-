using Authentication.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.DomainLayer.Entities;
using System.Security.Claims;
using Cryptography;
using Cryptography.Entities;
using Cryptography.Cryptors;
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Dtos;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.MassTransit;
using EventBus.Entities.Identity.User;
using EventBus.Entities.AppStatistics;
using NLog;

namespace Authentication.Api.Controllers
{
    [Route("api/v0/confirm")]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IJwtService _jwtService;
        private IUnitOfWork _unitOfWork;
        private Publisher _publisher;
        public ConfirmController(IJwtService jwtService, IUnitOfWork unitOfWork, Publisher publisher)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);

            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor
            .Crypt(principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Authentication)!.Value);

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value,
                Phone = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)!.Value,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            };

            UserRole? userRole = await _unitOfWork.UserRoles.GetByNameAsync("user");

            UserInfo userInfo = new UserInfo()
            {
                Id = Guid.NewGuid(),
                Nickname = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value,
                RoleId = userRole!.Id,
                Role = userRole,
                UserId = user.Id,
                User = user
            };

            if (await _unitOfWork.Users.GetByEmailAsync(user.Email) is not null ||
            await _unitOfWork.UserInfos.GetByNicknameAsync(userInfo.Nickname) is not null)
                throw new ConflictException<User>();

            await _unitOfWork.Users.AddAsync(user);
            UserInfoDto infoDto = await _unitOfWork.UserInfos.AddAsync(userInfo);

            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto);
            await _publisher.Send(new IdentityModelCreateUser()
            {
                UserId = user.Id,
                InfoId = userInfo.Id,
                Nickname = userInfo.Nickname,
                Email = user.Email,
                Phone = user.Phone,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                RoleName = userRole.Name
            });
            await _publisher.Send(new AppStatisticsCreateOrUpdate()
            {
                TotalUsers = 1,
                Date = DateTime.Now
            });

            _logger.Info("GET / {0}", nameof(TokenModel));
            return LingoMq.Responses.LingoMqResponse.OkResult(tokenModel);
        }
    }
}

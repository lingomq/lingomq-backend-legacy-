using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Dtos;
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Services;
using Authentication.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Authentication.BusinessLayer.MassTransit;
using System.IdentityModel.Tokens.Jwt;
using EventBus.Entities.Email;
using NLog;

namespace Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/v0/auth")]
    public class AuthenticationController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IJwtService _jwtService;
        private IUnitOfWork _unitOfWork;
        private Publisher _publisher;
        public AuthenticationController(IJwtService jwtService, IUnitOfWork unitOfWork, Publisher publisher)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInUpResponseModel model)
        {
            UserInfoDto? infoDto = await _unitOfWork.UserInfos.GetByNicknameAsync(model.Nickname!);
            User? user = await _unitOfWork.Users.GetByEmailAsync(model.Email!);

            if (infoDto is null || user is null)
                throw new NotFoundException<User>();

            if (!ValidationService.IsValidPassword(ref user, model.Password!))
                throw new InvalidDataException<User>();

            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto);

            await _publisher.Send(new EmailModelSignIn()
            {
                Email = model.Email!,
                Nickname = model.Nickname!,
                Subject = "Вход в аккаунт"
            });

            _logger.Info("POST /sign-in {0}", nameof(TokenModel));
            return LingoMq.Responses.LingoMqResponse.OkResult(tokenModel);
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignInUpResponseModel model)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(model.Email!) is not null ||
                await _unitOfWork.UserInfos.GetByNicknameAsync(model.Nickname!) is not null)
                throw new ConflictException<User>();

            // Generate a confirmation token 
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, model.Email!),
                new Claim(ClaimTypes.Name, model.Nickname!),
                new Claim(ClaimTypes.Authentication, model.Password!),
                new Claim(ClaimTypes.MobilePhone, model.Phone!),
                new Claim(ClaimTypes.Version, "email")
            };
            DateTime expiration = DateTime.Now.AddMinutes(600);
            JwtSecurityToken jwtEmailToken = _jwtService.CreateToken(claims, expiration);
            string emailToken = _jwtService.WriteToken(jwtEmailToken);

            await _publisher.Send(new EmailModelSignUp() { Email = model.Email!, Nickname = model.Nickname!,
                Token = emailToken, Subject = "Подтверждение аккаунта"});

            _logger.Info("POST /sign-up {0}", nameof(StatusCode));
            return LingoMq.Responses.LingoMqResponse.AcceptedResult();
        }

        [HttpGet("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string token)
        {        
            ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);
            if (principal is null)
                throw new InvalidTokenException<User>();

            UserInfoDto? infoDto = await _unitOfWork.UserInfos
                .GetByNicknameAsync(
                principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value);
            
            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto!);

            _logger.Info("POST /refresh-token {0}", nameof(TokenModel));
            return LingoMq.Responses.LingoMqResponse.OkResult(tokenModel);
        }
    }
}

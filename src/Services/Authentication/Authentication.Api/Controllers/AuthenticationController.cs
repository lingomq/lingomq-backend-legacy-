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
using Authentication.DomainLayer.Shared.Producers;

namespace Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/v0/auth")]
    public class AuthenticationController : ControllerBase
    {
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

            return LingoMq.Responses.StatusCode.OkResult(tokenModel);
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
                new Claim(ClaimTypes.MobilePhone, model.Phone!)
            };
            DateTime expiration = DateTime.Now.AddMinutes(600);
            string emailToken = _jwtService.CreateToken(claims, expiration).ToString();

            await _publisher.Send(new Confirmation() { Token = emailToken });

            return LingoMq.Responses.StatusCode.AcceptedResult();
        }

        [HttpGet("refresh-token")]
        [Authorize("user")]
        [Authorize("admin")]
        [Authorize("moderator")]
        public async Task<IActionResult> RefreshToken(string token)
        {        
            ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);
            if (principal is null)
                throw new InvalidTokenException<User>();

            UserInfoDto? infoDto = await _unitOfWork.UserInfos
                .GetByNicknameAsync(
                principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value);
            
            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto!);
            
            return LingoMq.Responses.StatusCode.OkResult(tokenModel);
        }

        [HttpGet]
        public IActionResult HelloWorld()
        {
            return LingoMq.Responses.StatusCode.OkResult("hello world");
        }
    }
}

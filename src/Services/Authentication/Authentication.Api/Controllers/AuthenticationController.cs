using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Dtos;
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Extensions;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Services;
using Authentication.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/v0/auth")]
    public class AuthenticationController : ControllerBase
    {
        private IJwtService _jwtService;
        private IUnitOfWork _unitOfWork;
        public AuthenticationController(IJwtService jwtService, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInUpResponseModel model)
        {
            UserInfoDto? infoDto = await _unitOfWork.UserInfos.GetByNicknameAsync(model.Nickname!);
            User? user = await _unitOfWork.Users.GetByEmailAsync(model.Email!);

            if (infoDto is null || user is null)
                throw new NotFoundException();

            if (!ValidationService.IsValidPassword(ref user, model.Password!))
                throw new BusinessLayer.Exceptions.InvalidDataException();

            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto);

            return Responses.StatusCode.OkResult(tokenModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignInUpResponseModel model)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(model.Nickname!) is not null ||
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

            // Send mail
            // Return result
            return Responses.StatusCode.AcceptedResult();
        }

        [HttpGet]
        [Authorize("user")]
        [Authorize("admin")]
        [Authorize("moderator")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            // Check validity token
            // append time
            // return token
        }
    }
}

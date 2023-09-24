using Authentication.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.DomainLayer.Entities;
using System.Security.Claims;

namespace Authentication.Api.Controllers
{
    [Route("api/v0/confirm")]
    [ApiController]
    public class ConfirmEmailToken : ControllerBase
    {
        private IJwtService _jwtService;
        private IUnitOfWork _unitOfWork;
        public ConfirmEmailToken(IJwtService jwtService, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailToken(string token)
        {
            ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);
           
            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Authentication)); 

            User user = new User()
            {
                Email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email),
                Phone = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone),
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            };

            List<UserRole> userRoles = await _unitOfWork.GetAsync();
            UserRole userRole = userRoles.FirstOrDefault(x => x.Name == "user");

            UserInfo userInfo = new UserInfo()
            {
                Nickname = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name),
                RoleId = userRole.Id,
                Role = userRole
            };

            if (await _unitOfWork.Users.GetByEmailAsync(user.Email) is not null ||
                await _unitOfWork.UserInfos.GetByNicknameAsync(userInfo.Nickname) is not null)
                throw new ConflictException<User>();

            await _unitOfWork.Users.AddAsync(user);
            UserInfoDto infoDto = await _unitOfWork.UserInfos.AddAsync(userInfo); 
            
            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto);
            // return result
            return Responses.StatusCode.OkResult(tokenModel);
        }
    }
}

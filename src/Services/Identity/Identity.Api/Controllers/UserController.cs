using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;
using EventBus.Entities.Identity;
using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Dtos;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.BusinessLayer.Exceptions.ServerExceptions;
using Identity.BusinessLayer.Extensions;
using Identity.BusinessLayer.MassTransit;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/v0/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublisherBase _publisher;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);
        public UserController(IUnitOfWork unitOfWork, PublisherBase publisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpGet]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get()
        {
            if (UserId == Guid.Empty)
                throw new UnauthorizedException<User>("Вы не авторизованы");
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(UserId);
            if (user is null)
                throw new NotFoundException<User>("Пользователь не найден");

            return LingoMq.Responses.StatusCode.OkResult(user);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid userId)
        {
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException<User>("Пользователь не найден");

            return LingoMq.Responses.StatusCode.OkResult(user);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            if (!userDto.Id.Equals(UserId))
                throw new ForbiddenException<User>("Вы не являетесь владельцем аккаунта");

            User user = userDto.ToUserModel();

            await _unitOfWork.Users.UpdateAsync(user);

            await _publisher.Send(new IdentityModelUpdateUser()
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.Phone
            });

            return LingoMq.Responses.StatusCode.OkResult(user, "succesfully update");
        }

        [HttpPut("credentials")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Update(string password)
        {
            if (UserId == Guid.Empty)
                throw new UnauthorizedException<User>("Вы не авторизованы");

            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor
            .Crypt(password);

            User user = new User()
            {
                Id = UserId,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            };

            await _unitOfWork.Users.UpdateCredentialsAsync(user); 
            
            await _publisher.Send(new IdentityModelUpdateUserCredentials()
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            });

            return LingoMq.Responses.StatusCode.OkResult(user, "password succesfully update");
        }
        [HttpDelete]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Delete()
        {
            if (UserId == Guid.Empty)
                throw new UnauthorizedException<User>("Вы не авторизованы");

            bool isRemove = await _unitOfWork.Users.DeleteAsync(UserId);

            if (!isRemove)
                throw new InternalServerException("Аккаунт не был удален");

            await _publisher.Send(new IdentityModelDeleteUser()
            {
                Id = UserId,
            });

            return LingoMq.Responses.StatusCode.OkResult("user has been successfully removed");
        }
        [HttpDelete("{userId}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            UserDto? user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user is null)
                throw new InvalidDataException<User>("Пользователя не существует"); 

            bool isRemove = await _unitOfWork.Users.DeleteAsync(userId);

            if (!isRemove)
                throw new InternalServerException("Ошибка. Аккаунт не был удален.");

            await _publisher.Send(new IdentityModelDeleteUser()
            {
                Id = userId,
            });

            return LingoMq.Responses.StatusCode.OkResult("user has been successfully removed");
        }
    }
}

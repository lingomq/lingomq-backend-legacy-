using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.MassTransit;
using Authentication.Api.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Services;
using Authentication.UnitTest.Common.Factories;
using Authentication.DomainLayer.Entities;
using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;
using System.Data;
using Dapper;
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Dtos;
using static MassTransit.ValidationResultExtensions;

namespace Authentication.UnitTest.Tests
{
    public class AuthenticationControllerTests
    {
        protected readonly IConfiguration _configuration;
        private readonly AuthenticationController _controller;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;
        private readonly IJwtService _jwtService;

        public AuthenticationControllerTests()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var provider = ServiceProviderFactory.Create(_configuration);
            Migrator.Migrate(provider);

            var publisher = provider.GetRequiredService<Publisher>();
            _connection = provider.GetRequiredService<IDbConnection>();

            _jwtService = new JwtService(_configuration);

            _unitOfWork = UnitOfWorkFactory.Create(provider);

            _controller = new AuthenticationController(_jwtService, _unitOfWork, publisher);
        }
        [Fact]
        public async Task POST_SignUp_ShouldBeOk()
        {
            // Arrange
            await RemoveData();
            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            // Act
            var result = await _controller.SignUp(model);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }
        [Fact]
        public async Task POST_SignUp_AlreadyExistedEmail_ShouldBeConflict()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(model.Password);

            Guid userId = Guid.NewGuid();
            await _unitOfWork.Users.AddAsync(new()
            { 
                Id = userId,
                Email = model.Email,
                Phone = model.Phone,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            });

            UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync("user");

            await _unitOfWork.UserInfos.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Nickname = "Generation",
                ImageUri = "uri",
                Additional = "add",
                IsRemoved = false,
                CreationalDate = DateTime.Now,
                Role = role,
                RoleId = role!.Id,
                UserId = userId
            });

            //Assert
            await Assert.ThrowsAsync<ConflictException<User>>(async () => await _controller.SignUp(model));
        }
        [Fact]
        public async Task POST_SignUp_AlreadyExistedNickname_ShouldBeConflict()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(model.Password);

            await _unitOfWork.Users.AddAsync(new()
            {
                Email = "Generation@mail.ru",
                Phone = model.Phone,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            });

            UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync("user");

            await _unitOfWork.UserInfos.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Nickname = model.Nickname,
                ImageUri = "uri",
                Additional = "add",
                IsRemoved = false,
                CreationalDate = DateTime.Now,
                Role = role,
                RoleId = role!.Id
            });

            //Assert
            await Assert.ThrowsAsync<ConflictException<User>>(async () => await _controller.SignUp(model));
        }
        [Fact]
        public async Task POST_SignIn_ShouldBeOk()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await SetUserDependency(model);

            // Act
            var result = await _controller.SignIn(model);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }
        [Fact]
        public async Task POST_SignIn_InvalidPassword_ShouldBeInvalidDataException()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await SetUserDependency(model);

            model.Password = "123";

            //Assert
            await Assert.ThrowsAsync<InvalidDataException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task POST_SignIn_IncorrectEmail_NotFoundException()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await SetUserDependency(model, email: "newmail@mail.ru");

            //Assert
            await Assert.ThrowsAsync<NotFoundException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task POST_SignIn_IncorrectNickname_NotFoundException()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await SetUserDependency(model, nickname: "SomeBodyHelp");
            
            //Assert
            await Assert.ThrowsAsync<NotFoundException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task GET_RefreshToken_ShouldBeOk()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await SetUserDependency(model);
            UserInfoDto? infoDto = await _unitOfWork.UserInfos.GetByNicknameAsync(model.Nickname!);
            TokenModel tokenModel = _jwtService.CreateTokenPair(infoDto!);

            // Act
            var result = await _controller.RefreshToken(tokenModel.RefreshToken!);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }
        [Fact]
        public async Task GET_RefreshToken_EmptyOrInvalidToken_ShouldBeInvalidTokenException()
        {
            // Arrange
            await RemoveData();
            await AddRoles();

            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidTokenException<User>>(async () => 
                await _controller.RefreshToken(""));
        }
        private async Task AddRoles()
        {
            await _unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "user" });
            await _unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "admin" });
            await _unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "moderator" });
        }
        private async Task SetUserDependency(SignInUpResponseModel model, string email = "",
            string nickname = "")
        {
            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(model.Password!);

            Guid userId = Guid.NewGuid();
            await _unitOfWork.Users.AddAsync(new()
            {
                Id = userId,
                Email = email != "" ? email : model.Email,
                Phone = model.Phone,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            });

            UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync("user");

            await _unitOfWork.UserInfos.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Nickname = nickname != "" ? nickname : model.Nickname,
                ImageUri = "uri",
                Additional = "add",
                IsRemoved = false,
                CreationalDate = DateTime.Now,
                RoleId = role!.Id,
                UserId = userId
            });
        }
        private async Task RemoveData()
        {
            _connection.Open();
            await _connection.ExecuteAsync("TRUNCATE TABLE users RESTART IDENTITY CASCADE;");
            await _connection.ExecuteAsync("TRUNCATE TABLE user_roles RESTART IDENTITY CASCADE;");
            await _connection.ExecuteAsync("TRUNCATE TABLE user_infos RESTART IDENTITY CASCADE;");
            _connection.Close();
        }
    }
}

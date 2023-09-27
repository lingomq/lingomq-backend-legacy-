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
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.Dtos;
using Authentication.UnitTest.Common.Services;

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
            await DatabaseHelper.RemoveDependencies(_connection);
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
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

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
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

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
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await DatabaseHelper.SetUserDependency(_unitOfWork, model);

            // Act
            var result = await _controller.SignIn(model);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }
        [Fact]
        public async Task POST_SignIn_InvalidPassword_ShouldBeInvalidDataException()
        {
            // Arrange
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await DatabaseHelper.SetUserDependency(_unitOfWork, model);

            model.Password = "123";

            //Assert
            await Assert.ThrowsAsync<InvalidDataException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task POST_SignIn_IncorrectEmail_NotFoundException()
        {
            // Arrange
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await DatabaseHelper.SetUserDependency(_unitOfWork, model, email: "newmail@mail.ru");

            //Assert
            await Assert.ThrowsAsync<NotFoundException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task POST_SignIn_IncorrectNickname_NotFoundException()
        {
            // Arrange
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await DatabaseHelper.SetUserDependency(_unitOfWork, model, nickname: "SomeBodyHelp");
            
            //Assert
            await Assert.ThrowsAsync<NotFoundException<User>>(async () => await _controller.SignIn(model));
        }
        [Fact]
        public async Task GET_RefreshToken_ShouldBeOk()
        {
            // Arrange
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            SignInUpResponseModel model = new SignInUpResponseModel()
            {
                Nickname = "AuthControllerTest",
                Email = "sampleEmail@mail.ru",
                Phone = "+79871234567",
                Password = "superpassword"
            };

            await DatabaseHelper.SetUserDependency(_unitOfWork, model);
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
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidTokenException<User>>(async () => 
                await _controller.RefreshToken(""));
        }
    }
}

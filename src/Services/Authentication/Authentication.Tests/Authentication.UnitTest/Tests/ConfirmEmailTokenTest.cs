using Authentication.Api.Controllers;
using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Exceptions;
using Authentication.BusinessLayer.MassTransit;
using Authentication.BusinessLayer.Models;
using Authentication.BusinessLayer.Services;
using Authentication.DomainLayer.Entities;
using Authentication.UnitTest.Common.Factories;
using Authentication.UnitTest.Common.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Authentication.UnitTest.Tests
{
    public class ConfirmEmailTokenTest
    {
        protected readonly IConfiguration _configuration;
        private readonly ConfirmController _controller;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;
        private readonly IJwtService _jwtService;

        public ConfirmEmailTokenTest()
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

            _controller = new ConfirmController(_jwtService, _unitOfWork, publisher);
        }
        [Fact]
        public async Task GET_ConfirmEmail_ShouldBeOk()
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

            // Act
            var result = await _controller.ConfirmEmail(emailToken);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((IStatusCodeActionResult)result).StatusCode);
        }
        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        public async Task GET_ConfirmEmail_EmptyOrInvalidToken_ShouldBeInvalidTokenException(string token)
        {
            // Arrange
            await DatabaseHelper.RemoveDependencies(_connection);
            await DatabaseHelper.AddRoles(_unitOfWork);

            // Act

            // Assert
            await Assert.ThrowsAsync<InvalidTokenException<User>>(async () =>
            await _controller.ConfirmEmail(token));
        }
    }

}

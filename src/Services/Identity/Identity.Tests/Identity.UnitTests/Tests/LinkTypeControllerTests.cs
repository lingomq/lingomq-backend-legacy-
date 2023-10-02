using Identity.Api.Controllers;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.MassTransit;
using Identity.UnitTests.Common.Factories;
using Identity.UnitTests.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Identity.UnitTests.Tests
{
    public class LinkTypeControllerTests
    {
        private readonly LinkTypeController _controller;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;
        private readonly JwtService _jwtService;
        
        public LinkTypeControllerTests()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            _jwtService = new JwtService(configuration);

            var provider = ServiceProviderFactory.Create(configuration);
            Migrator.Migrate(provider);

            _connection = provider.GetRequiredService<IDbConnection>();

            _unitOfWork = UnitOfWorkFactory.Create(provider);

            _controller = new LinkTypeController(_unitOfWork);
        }
    }
}

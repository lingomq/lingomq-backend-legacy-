using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Services;
using Microsoft.Extensions.Configuration;

namespace Authentication.UnitTest.Common.Services
{
    public class JwtHelper
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        public IJwtService JwtService;
        public JwtHelper() => JwtService = new JwtService(_configuration);

    }
}

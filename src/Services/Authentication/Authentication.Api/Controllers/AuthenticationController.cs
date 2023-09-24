using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // Check is exist
            // Generate token
            // Send mail
            // Return result
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignInUpResponseModel model)
        {
            // Check is not-existance
            // Generate a confirmation token 
            // Send mail
            // Return result
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

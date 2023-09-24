using Authentication.BusinessLayer.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // Validating token
            // append a data
            // return result
        }
    }
}

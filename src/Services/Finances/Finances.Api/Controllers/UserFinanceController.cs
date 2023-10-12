using Finances.Api.Common;
using Finances.BusinessLayer.Contracts;
using Finances.BusinessLayer.Exceptions.ClientExceptions;
using Finances.BusinessLayer.Models.YooKassa;
using Finances.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finances.Api.Controllers
{
    [Route("api/finances/user")]
    [ApiController]
    public class UserFinanceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private Guid UserId => new(User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!);
        public UserFinanceController(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Roles = AccessRoles.User)]
        public async Task<IActionResult> Get()
        {
            List<UserFinance> finances = await _unitOfWork.UserFinances.GetByUserIdAsync(UserId);
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(int range)
        {
            List<UserFinance> finances = await _unitOfWork.UserFinances.GetAsync(range);
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(Guid userId)
        {
            if (await _unitOfWork.Users.GetAsync(userId) is null)
                throw new NotFoundException<User>();

            List<UserFinance> finances = await _unitOfWork.UserFinances.GetByUserIdAsync(userId);
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("status/{userId}&{financeId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetStatus(Guid userId, Guid financeId)
        {
            User? user = await _unitOfWork.Users.GetAsync(userId);
            Finance? finance = await _unitOfWork.Finances.GetAsync(financeId);
            string status = "actually";

            if (user is null) throw new NotFoundException<User>();
            if (finance is null) throw new NotFoundException<Finance>();

            List<UserFinance> finances = await _unitOfWork.UserFinances.GetByUserIdAsync(userId);
            UserFinance? currentFinance = finances.FirstOrDefault(f => f.UserId == userId && f.FinanceId == financeId);

            if (currentFinance is null)
                throw new NotFoundException<UserFinance>();

            if (currentFinance.EndSubscriptionDate < DateTime.UtcNow)
                status = "exceed";

            return LingoMqResponse.OkResult(new { Status = status, Data = currentFinance });
        }

        [HttpPost("confirm")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> ConfirmPayment(Guid userId, Guid financeId, YooKassaSuccessResponse response)
        {
            if (await _unitOfWork.Users.GetAsync(userId) is null) throw new NotFoundException<User>();
            if (await _unitOfWork.Finances.GetAsync(financeId) is null) throw new NotFoundException<Finance>();

            if (!await _paymentService.ConfirmPayment(Guid.Parse(response.Id!)) ||
                !await _paymentService.ConfirmPaymentData(userId, financeId, Guid.Parse(response.Id!)))
                throw new ForbiddenException<UserFinance>();

            UserFinance finance = new UserFinance()
            {
                UserId = userId,
                FinanceId = financeId,
                CreationDate = DateTime.UtcNow,
                EndSubscriptionDate = DateTime.UtcNow.AddDays(30)
            };

            List<UserFinance> finances = await _unitOfWork.UserFinances.GetByUserIdAsync(userId);
            UserFinance? currentFinance = finances.FirstOrDefault(f => f.UserId == userId && f.FinanceId == financeId);

            if (currentFinance is not null)
                await _unitOfWork.UserFinances.CreateAsync(finance);
            else
                await _unitOfWork.UserFinances.UpdateAsync(finance);

            return LingoMqResponse.AcceptedResult();
        }
    }
}

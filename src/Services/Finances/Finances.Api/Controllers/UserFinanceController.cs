using Finances.Api.Common;
using Finances.BusinessLayer.Contracts;
using Finances.BusinessLayer.Exceptions.ClientExceptions;
using Finances.BusinessLayer.Models.YooKassa;
using Finances.DomainLayer.Entities;
using LingoMqResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Claims;

namespace Finances.Api.Controllers
{
    [Route("api/v0/finances/user")]
    [ApiController]
    public class UserFinanceController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
            _logger.Info("GET / {0}", nameof(List<UserFinance>));
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(int range)
        {
            List<UserFinance> finances = await _unitOfWork.UserFinances.GetAsync(range);
            _logger.Info("GET /all/{range} {0}", nameof(List<UserFinance>));
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(Guid userId)
        {
            if (await _unitOfWork.Users.GetAsync(userId) is null)
                throw new NotFoundException<User>();

            List<UserFinance> finances = await _unitOfWork.UserFinances.GetByUserIdAsync(userId);
            _logger.Info("GET /{userId} {0}", nameof(List<UserFinance>));
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

            _logger.Info("GET /status/{userId}&{financeId} {0}", nameof(UserFinance));
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

            _logger.Info("POST /confirm {0}", nameof(UserFinance));
            return LingoMqResponse.AcceptedResult();
        }
    }
}

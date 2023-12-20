using Finances.Api.Common;
using Finances.BusinessLayer.Contracts;
using Finances.BusinessLayer.Exceptions.ClientExceptions;
using Finances.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Responses;

namespace Finances.Api.Controllers
{
    [Route("api/v0/finances")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public FinanceController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range)
        {
            List<Finance> finances = await _unitOfWork.Finances.GetAsync(range);
            _logger.Info("GET /all/{range} {0}", nameof(List<Finance>));
            return LingoMqResponse.OkResult(finances);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            Finance? finance = await _unitOfWork.Finances.GetAsync(id);
            if (finance is null)
                throw new NotFoundException<Finance>();

            _logger.Info("GET /{id} {0}", nameof(Finance));
            return LingoMqResponse.OkResult(finance);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Create(Finance finance)
        {
            await _unitOfWork.Finances.CreateAsync(finance);

            _logger.Info("POST / {0}", nameof(Finance));
            return LingoMqResponse.AcceptedResult(finance);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(Finance finance)
        {
            if (await _unitOfWork.Finances.GetAsync(finance.Id) is null)
                throw new NotFoundException<Finance>();

            await _unitOfWork.Finances.UpdateAsync(finance);

            _logger.Info("PUT / {0}", nameof(Finance));
            return LingoMqResponse.AcceptedResult(finance);
        }

        [HttpDelete("id")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.Finances.GetAsync(id) is null)
                throw new NotFoundException<Finance>();

            await _unitOfWork.Finances.DeleteAsync(id);

            _logger.Info("DELETE / {0}", nameof(Finance));
            return LingoMqResponse.AcceptedResult();
        }
    }
}

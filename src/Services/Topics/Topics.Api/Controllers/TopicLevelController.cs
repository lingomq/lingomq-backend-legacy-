using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Topics.Api.Common;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Exceptions.ClientExceptions;
using Topics.DomainLayer.Entities;

namespace Topics.Api.Controllers
{
    [Route("api/topics/level")]
    [ApiController]
    public class TopicLevelController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public TopicLevelController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<TopicLevel> levels = await _unitOfWork.TopicLevels.GetAsync(range);

            _logger.Info("GET /all/{range} {0}", nameof(List<TopicLevel>));
            return LingoMqResponse.OkResult(levels);
        }

        [HttpGet("topic-id/{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            TopicLevel? level = await _unitOfWork.TopicLevels.GetByIdAsync(id);
            if (level is null)
                throw new NotFoundException<TopicLevel>();

            _logger.Info("GET /{id} {0}", nameof(TopicLevel));
            return LingoMqResponse.OkResult(level);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Create(TopicLevel level)
        {
            await _unitOfWork.TopicLevels.AddAsync(level);
            _logger.Info("POST / {0}", nameof(TopicLevel));
            return LingoMqResponse.AcceptedResult(level);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Update(TopicLevel level)
        {
            if (await _unitOfWork.TopicLevels.GetByIdAsync(level.Id) is null)
                throw new NotFoundException<TopicLevel>();

            await _unitOfWork.TopicLevels.UpdateAsync(level);
            _logger.Info("PUT / {0}", nameof(TopicLevel));
            return LingoMqResponse.AcceptedResult(level);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.TopicLevels.GetByIdAsync(id) is null)
                throw new NotFoundException<TopicLevel>();

            await _unitOfWork.TopicLevels.DeleteAsync(id);
            _logger.Info("DELETE /{id} {0}", nameof(TopicLevel));
            return LingoMqResponse.AcceptedResult();
        }
    }
}

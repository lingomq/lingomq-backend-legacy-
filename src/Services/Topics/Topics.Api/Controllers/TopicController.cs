using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Topics.Api.Common;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Dtos;
using Topics.BusinessLayer.Exceptions.ClientExceptions;
using Topics.BusinessLayer.Extensions;
using Topics.DomainLayer.Entities;

namespace Topics.Api.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public TopicController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range)
        {
            List<Topic> topics = await _unitOfWork.Topics.GetAsync(range);
            _logger.Info("GET /all/{range} {0}", nameof(List<Topic>));
            return LingoMqResponse.OkResult(topics);
        }

        [HttpGet("language/{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByLanguageId(Guid id)
        {
            List<Topic> topics = await _unitOfWork.Topics.GetByLanguageIdAsync(id);
            _logger.Info("GET /language/{id} {0}", nameof(List<Topic>));
            return LingoMqResponse.OkResult(topics);
        }

        [HttpGet("level/{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByLevelId(Guid id)
        {
            List<Topic> topics = await _unitOfWork.Topics.GetByTopicLevelIdAsync(id);
            _logger.Info("GET /level/{id} {0}", nameof(List<Topic>));
            return LingoMqResponse.OkResult(topics);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            Topic? topic = await _unitOfWork.Topics.GetByIdAsync(id);
            if (topic is null)
                throw new NotFoundException<Topic>();

            _logger.Info("GET /{id} {0}", nameof(Topic));
            return LingoMqResponse.OkResult(topic);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Create(TopicDto topic)
        {
            await _unitOfWork.Topics.AddAsync(topic.ToModel());
            _logger.Info("POST / {0}", nameof(Topic));
            return LingoMqResponse.AcceptedResult(topic);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Update(TopicDto topic)
        {
            if (await _unitOfWork.Topics.GetByIdAsync(topic.Id) is null)
                throw new InvalidDataException<Topic>(new string[] { "Id" });

            await _unitOfWork.Topics.UpdateAsync(topic.ToModel());
            _logger.Info("PUT / {0}", nameof(Topic));
            return LingoMqResponse.AcceptedResult(topic);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.Topics.GetByIdAsync(id) is null)
                throw new InvalidDataException<Topic>(new string[] { "Id" });

            await _unitOfWork.Topics.DeleteAsync(id);
            _logger.Info("DELETE /{id} {0}", nameof(Topic));
            return LingoMqResponse.AcceptedResult();
        }
    }
}

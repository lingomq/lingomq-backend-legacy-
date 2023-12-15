using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Topics.Api.Common;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Dtos;
using Topics.BusinessLayer.Exceptions.ClientExceptions;
using Topics.BusinessLayer.Extensions;
using Topics.BusinessLayer.Models;
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

        [HttpGet("all/skip/{skip}/take/{take}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int skip = 0, int take = int.MaxValue)
        {
            List<Topic> topics = await _unitOfWork.Topics.GetAsync(skip, take);
            _logger.Info("GET /all/{range} {0}", nameof(List<Topic>));
            return LingoMqResponse.OkResult(topics);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetWithFilters(TopicFilters topicFilters)
        {
            List<Topic> topics = await _unitOfWork.Topics.GetByTopicFiltersAsync(topicFilters);
            return LingoMqResponse.OkResult(topics);
        }

        [HttpGet("topic-id/{id}")]
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

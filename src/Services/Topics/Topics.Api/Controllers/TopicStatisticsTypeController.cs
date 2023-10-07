using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Topics.Api.Common;
using Topics.BusinessLayer.Contracts;
using Topics.BusinessLayer.Exceptions.ClientExceptions;
using Topics.DomainLayer.Entities;

namespace Topics.Api.Controllers
{
    [Route("api/topics/types")]
    [ApiController]
    public class TopicStatisticsTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicStatisticsTypeController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<TopicStatisticsType> types = await _unitOfWork.StatisticsTypes.GetAsync(range);

            return LingoMqResponse.OkResult(types);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Create(TopicStatisticsType type)
        {
            await _unitOfWork.StatisticsTypes.AddAsync(type);
            return LingoMqResponse.AcceptedResult(type);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Update(TopicStatisticsType type)
        {
            if (await _unitOfWork.StatisticsTypes.GetByIdAsync(type.Id) is null)
                throw new InvalidDataException<TopicStatisticsType>(new string[] { "Id" });

            await _unitOfWork.StatisticsTypes.UpdateAsync(type);
            return LingoMqResponse.AcceptedResult(type);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.StatisticsTypes.GetByIdAsync(id) is null)
                throw new InvalidDataException<TopicStatisticsType>(new string[] { "Id" });

            await _unitOfWork.StatisticsTypes.DeleteAsync(id);
            return LingoMqResponse.AcceptedResult();
        }
    }
}
 
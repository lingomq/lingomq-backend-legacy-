using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Identity.Api.Controllers
{
    [Route("api/v0/link-type")]
    [ApiController]
    public class LinkTypeController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        public LinkTypeController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<LinkType> types = await _unitOfWork.LinkTypes.GetAsync(range);
            _logger.Info("GET /all/{range} {0}", nameof(List<LinkType>));
            return LingoMq.Responses.LingoMqResponse.OkResult(types);
        }

        [HttpGet("{linkId}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(Guid linkId)
        {
            LinkType? type = await _unitOfWork.LinkTypes.GetByIdAsync(linkId);

            if (type is null)
                throw new NotFoundException<LinkType>();

            _logger.Info("GET /{linkId} {0}", nameof(LinkType));
            return LingoMq.Responses.LingoMqResponse.OkResult(type);
        }
        [HttpGet("name/{name}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(string name)
        {
            LinkType? type = await _unitOfWork.LinkTypes.GetByNameAsync(name);

            if (type is null)
                throw new NotFoundException<LinkType>();

            _logger.Info("GET /name/{name} {0}", nameof(LinkType));
            return LingoMq.Responses.LingoMqResponse.OkResult(type);
        }
        [HttpPost]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Create(LinkType linkType)
        {
            await _unitOfWork.LinkTypes.AddAsync(linkType);

            _logger.Info("POST / {0}", nameof(LinkType));
            return LingoMq.Responses.LingoMqResponse.OkResult(linkType);
        }
        [HttpPut]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Update(LinkType linkType)
        {
            await _unitOfWork.LinkTypes.UpdateAsync(linkType);

            _logger.Info("PUT / {0}", nameof(LinkType));
            return LingoMq.Responses.LingoMqResponse.OkResult(linkType);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Delete(Guid id)
        {
            LinkType? type = await _unitOfWork.LinkTypes.GetByIdAsync(id);

            if (type is null)
                throw new NotFoundException<LinkType>();

            await _unitOfWork.LinkTypes.DeleteAsync(id);

            _logger.Info("DELETE /{id} {0}", nameof(LinkType));
            return LingoMq.Responses.LingoMqResponse.OkResult(type);
        }
    }
}

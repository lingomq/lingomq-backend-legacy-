using EventBus.Entities.Identity.UserRoles;
using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.BusinessLayer.Exceptions.ServerExceptions;
using Identity.BusinessLayer.MassTransit;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Identity.Api.Controllers
{
    [Route("api/v0/identity/user/role")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublisherBase _publisher;
        public UserRoleController(IUnitOfWork unitOfWork, PublisherBase publisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<UserRole> roles = await _unitOfWork.UserRoles.GetAsync(range);
            _logger.Info("GET /all/{range} {0}", nameof(List<UserRole>));
            return LingoMq.Responses.LingoMqResponse.OkResult(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> Get(Guid id)
        {
            UserRole? role = await _unitOfWork.UserRoles.GetByIdAsync(id);

            if (role is null)
                throw new NotFoundException<UserRole>();

            _logger.Info("GET /{id} {0}", nameof(UserRole));
            return LingoMq.Responses.LingoMqResponse.OkResult(role);
        }

        [HttpGet("name/{name}")]
        [Authorize(Roles = AccessRoles.Staff)]
        public async Task<IActionResult> GetByName(string name)
        {
            UserRole? role = await _unitOfWork.UserRoles.GetByNameAsync(name);

            if (role is null)
                throw new NotFoundException<UserRole>();

            _logger.Info("GET /name/{name} {0}", nameof(UserRole));
            return LingoMq.Responses.LingoMqResponse.OkResult(role);
        }
        [HttpPost]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Create(UserRole role)
        {
            await _unitOfWork.UserRoles.AddAsync(role);

            await _publisher.Send(new IdentityModelUserRoleAdd()
            {
                Id = role.Id,
                Name = role.Name
            });

            _logger.Info("POST / {0}", nameof(UserRole));
            return LingoMq.Responses.LingoMqResponse.OkResult(role, "role has been succesfully appended");
        }
        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(UserRole role)
        {
            await _unitOfWork.UserRoles.UpdateAsync(role);

            await _publisher.Send(new IdentityModelUserRoleUpdate()
            {
                Id = role.Id,
                Name = role.Name
            });

            _logger.Info("PUT / {0}", nameof(UserRole));
            return LingoMq.Responses.LingoMqResponse.OkResult(role, "role has been succesfully updated");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserRole? role = await _unitOfWork.UserRoles.GetByIdAsync(id);
            if (role is null)
                throw new NotFoundException<UserRole>("role wasn't found");

            bool isRemoved = await _unitOfWork.UserRoles.DeleteAsync(id);

            if (!isRemoved)
                throw new InternalServerException("role wasn't deleted. try again");

            await _publisher.Send(new IdentityModelUserRoleDelete()
            {
                Id = role.Id
            });

            _logger.Info("DELETE /{id} {0}", nameof(UserRole));
            return LingoMq.Responses.LingoMqResponse.OkResult(role, "role has been succesfully removed");
        }
    }
}

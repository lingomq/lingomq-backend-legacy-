using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Words.Api.Common;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Exceptions.ClientExceptions;
using Words.DomainLayer.Entities;

namespace Words.Api.Controllers
{
    [Route("api/words/user-word-types")]
    [ApiController]
    public class UserWordTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserWordTypeController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;
        private Guid UserId => new Guid(User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
            .FirstOrDefault()?.Value!);

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range)
        {
            List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetAsync(range);
            return LingoMq.Responses.LingoMqResponse.OkResult(userWordTypes);
        }

        [HttpGet("{wordId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByWordId(Guid wordId)
        {
            List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetByUserIdAsync(wordId);
            return LingoMq.Responses.LingoMqResponse.OkResult(userWordTypes);
        }

        [HttpGet("typeId/{typeId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByTypeId(Guid typeId)
        {
            List<UserWordType> userWordTypes = await _unitOfWork.UserWordTypes.GetByTypeIdAsync(typeId);
            return LingoMq.Responses.LingoMqResponse.OkResult(userWordTypes);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Create(UserWordType userWordType)
        {
            await _unitOfWork.UserWordTypes.AddAsync(userWordType);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult(userWordType);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserWordType? userWordType = await _unitOfWork.UserWordTypes.GetByIdAsync(id);
            if (userWordType is null)
                throw new InvalidDataException<UserWordType>(parameters: new string[] { "id" });
            if (userWordType.UserWord!.User!.Id != UserId)
                throw new ForbiddenException<UserWordType>();

            await _unitOfWork.UserWordTypes.DeleteAsync(id);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult("UserWordType is succesfully remove");
        }
    }
}

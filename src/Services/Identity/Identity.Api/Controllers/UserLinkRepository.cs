using Identity.Api.Common;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Exceptions.ClientExceptions;
using Identity.DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/user/link")]
    [ApiController]
    public class UserLinkRepository : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLinkRepository(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("info-id/{userInfoId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> GetByUserInfoId(Guid userInfoId)
        {
            UserInfo? info = await _unitOfWork.UserInfos.GetByUserIdAsync(userInfoId);

            if (info is null)
                throw new NotFoundException<UserInfo>("The user info not found");

            List<UserLink> links = await _unitOfWork.UserLinks.GetByUserInfoIdAsync(userInfoId);

            return LingoMq.Responses.LingoMqResponse.OkResult(links);
        }

        [HttpGet("{linkId}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid linkId)
        {
            List<UserLink> links = await _unitOfWork.UserLinks.GetAllByIdAsync(linkId);

            return LingoMq.Responses.LingoMqResponse.OkResult(links);
        }
        [HttpPost]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Create(UserLink link)
        {
            LinkType? linkType = await _unitOfWork.LinkTypes.GetByIdAsync(link.LinkId);

            if (linkType is null)
                throw new NotFoundException<UserLink>(link, "The received link_type isn't found");

            UserInfo? info = await _unitOfWork.UserInfos.GetByIdAsync(link.UserInfoId);

            if (info is null)
                throw new NotFoundException<UserLink>(link, "The received user_info isn't found");

            await _unitOfWork.UserLinks.AddAsync(link);

            return LingoMq.Responses.LingoMqResponse.OkResult(link);
        }
        [HttpPut]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Update(UserLink link)
        {
            LinkType? linkType = await _unitOfWork.LinkTypes.GetByIdAsync(link.LinkId);

            if (linkType is null)
                throw new NotFoundException<UserLink>(link, "The received link_type isn't found");

            UserInfo? info = await _unitOfWork.UserInfos.GetByIdAsync(link.UserInfoId);

            if (info is null)
                throw new NotFoundException<UserLink>(link, "The received user_info isn't found");

            await _unitOfWork.UserLinks.UpdateAsync(link);

            return LingoMq.Responses.LingoMqResponse.OkResult(link);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserLink? link = await _unitOfWork.UserLinks.GetByIdAsync(id);

            if (link is null)
                throw new NotFoundException<UserLink>("The user_link doesn't found");

            await _unitOfWork.UserLinks.DeleteAsync(id);

            return LingoMq.Responses.LingoMqResponse.OkResult(link);
        }
    }
}

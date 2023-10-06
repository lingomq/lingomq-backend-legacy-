﻿using Achievements.Api.Common;
using Achievements.BusinessLayer.Contracts;
using Achievements.BusinessLayer.Exceptions.ClientExceptions;
using Achievements.DomainLayer.Entities;
using LingoMq.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.Api.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AchievementController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range)
        {
            List<Achievement> achievements = await _unitOfWork.Achievements.GetAsync(range);
            return LingoMqResponse.OkResult(achievements);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            Achievement? achievement = await _unitOfWork.Achievements.GetByIdAsync(id);
            if (achievement is null)
                throw new NotFoundException<Achievement>();

            return LingoMqResponse.OkResult(achievement);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Create(Achievement achievement)
        {
            await _unitOfWork.Achievements.CreateAsync(achievement);
            return LingoMqResponse.AcceptedResult(achievement);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(Achievement achievement)
        {
            if (await _unitOfWork.Achievements.GetByIdAsync(achievement.Id) is null)
                throw new InvalidDataException<Achievement>("Data hasn't been found");

            await _unitOfWork.Achievements.UpdateAsync(achievement);

            return LingoMqResponse.AcceptedResult(achievement);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.Achievements.GetByIdAsync(id) is null)
                throw new InvalidDataException<Achievement>("Data hasn't been found");

            await _unitOfWork.Achievements.DeleteAsync(id);

            return LingoMqResponse.AcceptedResult();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.Api.Common;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Exceptions.ClientExceptions;
using Words.DomainLayer.Entities;

namespace Words.Api.Controllers
{
    [Route("api/words/word-types")]
    [ApiController]
    public class WordTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public WordTypeController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<WordType> wordTypes = await _unitOfWork.WordTypes.GetAsync(range);
            return LingoMq.Responses.LingoMqResponse.OkResult(wordTypes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            WordType? wordType = await _unitOfWork.WordTypes.GetByIdAsync(id);
            if (wordType is null)
                throw new NotFoundException<WordType>();

            return LingoMq.Responses.LingoMqResponse.OkResult(wordType);
        }

        [HttpPost]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Create(WordType wordType)
        {
            await _unitOfWork.WordTypes.AddAsync(wordType);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult(wordType);
        }

        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(WordType wordType)
        {
            if (await _unitOfWork.WordTypes.GetByIdAsync(wordType.Id) is null)
                throw new NotFoundException<WordType>();

            await _unitOfWork.WordTypes.UpdateAsync(wordType);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult(wordType);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.WordTypes.GetByIdAsync(id) is null)
                throw new InvalidDataException<WordType>(parameters: new string[] { "id" });

            await _unitOfWork.WordTypes.DeleteAsync(id);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult("WordType is succesfully remove");
        }
    }
}

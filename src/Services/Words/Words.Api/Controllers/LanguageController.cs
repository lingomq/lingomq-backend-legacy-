using EventBus.Entities.Words;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.Api.Common;
using Words.BusinessLayer.Contracts;
using Words.BusinessLayer.Exceptions.ClientExceptions;
using Words.BusinessLayer.MassTransit;
using Words.DomainLayer.Entities;

namespace Words.Api.Controllers
{
    [Route("api/v0/words/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublisherBase _publisher;
        public LanguageController(IUnitOfWork unitOfWork, PublisherBase publisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        [HttpGet("all/{range}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(int range = int.MaxValue)
        {
            List<Language> languages = await _unitOfWork.Languages.GetAsync(range);
            return LingoMq.Responses.LingoMqResponse.OkResult(languages);
        }
        [HttpGet("name/{name}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(string name)
        {
            Language? language = await _unitOfWork.Languages.GetByNameAsync(name);
            if (language is null)
                throw new NotFoundException<Language>();

            return LingoMq.Responses.LingoMqResponse.OkResult(language);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = AccessRoles.All)]
        public async Task<IActionResult> Get(Guid id)
        {
            Language? language = await _unitOfWork.Languages.GetByIdAsync(id);
            if (language is null)
                throw new NotFoundException<Language>();

            return LingoMq.Responses.LingoMqResponse.OkResult(language);
        }
        [HttpPost]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Create(Language language)
        {
            await _unitOfWork.Languages.AddAsync(language);

            await _publisher.Send(new WordsLanguageCreate()
            {
                Id = language.Id,
                Name = language.Name
            });
            return LingoMq.Responses.LingoMqResponse.AcceptedResult(language);
        }
        [HttpPut]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Update(Language language)
        {
            if (await _unitOfWork.Languages.GetByIdAsync(language.Id) is null)
                throw new NotFoundException<Language>();

            await _unitOfWork.Languages.UpdateAsync(language);
            await _publisher.Send(new WordsLanguageUpdate()
            {
                Id = language.Id,
                Name = language.Name
            });
            return LingoMq.Responses.LingoMqResponse.AcceptedResult(language);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = AccessRoles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _unitOfWork.Languages.GetByIdAsync(id) is null)
                throw new NotFoundException<Language>();

            await _publisher.Send(new WordsLanguageDelete()
            {
                Id = id,
            });
            await _unitOfWork.Languages.DeleteAsync(id);
            return LingoMq.Responses.LingoMqResponse.AcceptedResult("Language has been removed");
        }
    }
}

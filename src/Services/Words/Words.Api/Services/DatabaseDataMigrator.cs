using Words.BusinessLayer.Contracts;
using Words.DomainLayer.Entities;

namespace Words.Api.Services
{
    public class DatabaseDataMigrator : IDatabaseDataMigrator
    {
        private readonly IUnitOfWork _unitOfWork;
        public DatabaseDataMigrator(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task AddLanguages()
        {
            List<string> languageNames = new List<string>() { "english", "russian" };
            var languagesFromDatabase = await _unitOfWork.Languages.GetAsync(10);

            foreach (var languageName in languagesFromDatabase.Select(x => x.Name))
            {
                if (languageNames.Contains(languageName!))
                    languageNames.Remove(languageName!);
            }

            foreach (string languageName in languageNames)
                await _unitOfWork.Languages.AddAsync(new Language() { Id = Guid.NewGuid(), Name = languageName });
        }

        public async Task AddWordTypes()
        {
            List<string> wordTypeNames = new List<string>() { "important", "usual" };
            var wordTypeFromDatabase = await _unitOfWork.WordTypes.GetAsync(10);

            foreach (var wordTypeName in wordTypeFromDatabase.Select(x => x.TypeName))
            {
                if (wordTypeNames.Contains(wordTypeName!))
                    wordTypeNames.Remove(wordTypeName!);
            }

            foreach (string wordTypeName in wordTypeNames)
                await _unitOfWork.WordTypes.AddAsync(new WordType() { Id = Guid.NewGuid(), TypeName = wordTypeName });
        }

        public async Task DoWork()
        {
            await AddLanguages();
            await AddWordTypes();
        }
    }
}

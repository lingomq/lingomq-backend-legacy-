using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.Api.Services
{
    public class DatabaseDataMigrator : IDatabaseDataMigrator
    {
        private readonly IUnitOfWork _unitOfWork;
        public DatabaseDataMigrator(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task AddTopicStatisticsTypeAsync()
        {
            List<string> statisticsTypeNames = new List<string>() { "views", "likes" };
            var statisticsTypesFromDatabase = await _unitOfWork.Languages.GetAsync(10);

            foreach (var statisticsName in statisticsTypesFromDatabase.Select(x => x.Name))
            {
                if (statisticsTypeNames.Contains(statisticsName!))
                    statisticsTypeNames.Remove(statisticsName!);
            }

            foreach (string statisticsName in statisticsTypeNames)
                await _unitOfWork.StatisticsTypes.AddAsync(new TopicStatisticsType() { Id = Guid.NewGuid(), TypeName = statisticsName });
        }

        public async Task AddLanguagesAsync()
        {
            List<string> languageNames = new List<string>() { "all", "english", "russian", "deutsch", "french", "japanese" };
            var languagesFromDatabase = await _unitOfWork.Languages.GetAsync(10);

            foreach (var languageName in languagesFromDatabase.Select(x => x.Name))
            {
                if (languageNames.Contains(languageName!))
                    languageNames.Remove(languageName!);
            }

            foreach (string languageName in languageNames)
                await _unitOfWork.Languages.AddAsync(new Language() { Id = Guid.NewGuid(), Name = languageName });
        }

        public async Task AddTopicLevelAsync()
        {
            List<string> levelNames = new List<string>() { "info", "beginners", "intermediate", "advanced" };
            var levelsFromDatabase = await _unitOfWork.TopicLevels.GetAsync(10);

            foreach (var levelName in levelsFromDatabase.Select(x => x.LevelName))
            {
                if (levelNames.Contains(levelName!))
                    levelNames.Remove(levelName!);
            }

            foreach (string levelName in levelNames)
                await _unitOfWork.TopicLevels.AddAsync(new TopicLevel() { Id = Guid.NewGuid(), LevelName = levelName });
        }

        public async Task MigrateAsync()
        {
            await AddLanguagesAsync();
            await AddTopicLevelAsync();
            await AddTopicStatisticsTypeAsync();
        }
    }
}

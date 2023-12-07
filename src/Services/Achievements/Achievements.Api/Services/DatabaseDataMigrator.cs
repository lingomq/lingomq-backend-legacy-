﻿using Achievements.BusinessLayer.Contracts;
using Achievements.DomainLayer.Entities;

namespace Achievements.Api.Services
{
    public class DatabaseDataMigrator : IDatabaseDataMigrator
    {
        private readonly IUnitOfWork _unitOfWork;
        public DatabaseDataMigrator(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task AddAchievementsAsync()
        {
            List<Achievement> achievements = new List<Achievement>()
            {
                new Achievement()
                {
                    Id = Guid.NewGuid(),
                    Name = "Начало начал",
                    Content = "Добавить первое слово",
                    ImageUri = "data:image/png;base64,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"
                },
                new Achievement()
                {
                    Id = Guid.NewGuid(),
                    Name = "Работаем!",
                    Content = "Зарегистрироваться",
                    ImageUri = "data:image/jpeg;base64,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"
                }
            };

            var achievementsFromDatabase = await _unitOfWork.Achievements.GetAsync(10);

            foreach (var achievementName in achievementsFromDatabase.Select(x => x.Name))
            {
                if (achievements.Select(x => x.Name).Contains(achievementName!))
                    achievements.Remove(achievements.FirstOrDefault(x => x.Name == achievementName)!);
            }

            foreach (Achievement achievement in achievements)
                await _unitOfWork.Achievements.CreateAsync(achievement);
        }

        public async Task MigrateAsync()
        {
            await AddAchievementsAsync();
        }
    }
}

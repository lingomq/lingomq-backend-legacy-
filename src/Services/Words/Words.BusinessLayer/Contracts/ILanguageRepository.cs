﻿using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {
        Task<Language?> GetByName(string name);
    }
}
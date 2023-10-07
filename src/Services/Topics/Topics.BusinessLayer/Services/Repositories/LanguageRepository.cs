using System.Data;
using Topics.BusinessLayer.Contracts;
using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Services.Repositories
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", name as \"Name\" " +
            "FROM languages ";
        private readonly static string GetRange = Get +
            "LIMIT @Count";
        private readonly static string GetById = Get +
            "WHERE id = @Id";
        private readonly static string GetByLanguageName = Get +
            "WHERE name = @Name";
        private readonly static string Create =
            "INSERT INTO languages (id, name) " +
            "VALUES (@Id, @Name)";
        private readonly static string Update =
            "UPDATE languages " +
            "SET name = @Name " +
            "WHERE id = @Id";
        private readonly static string Delete =
            "DELETE FROM languages " +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public LanguageRepository(IDbConnection connection) : base(connection) =>
            _connection = connection;

        public async Task AddAsync(Language entity)
        {
            await ExecuteByTemplateAsync(Create, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteByTemplateAsync(Delete, new { Id = id });
        }

        public async Task<List<Language>> GetAsync(int range = int.MaxValue)
        {
            return await GetByQueryAsync(GetRange, new { Count = range });
        }

        public async Task<Language?> GetByIdAsync(Guid id)
        {
            IEnumerable<Language> languages = await GetByQueryAsync(GetById, new { Id = id });
            return languages.FirstOrDefault();
        }

        public async Task UpdateAsync(Language entity)
        {
            await ExecuteByTemplateAsync(Update, entity);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace DataAccess.EntityFramework.Extensions;
public static class LanguageExtensions
{
    public static async Task<Language?> GetByIdAsync(this DbSet<Language> languages, Guid id) =>
        await languages.FirstOrDefaultAsync(x => x.Id == id);

    public static async Task DeleteAsync(this DbSet<Language> languages, Guid id)
    {
        Language language = await languages.FirstAsync(x => x.Id == id);
        languages.Remove(language);
    }

    public static async Task<List<Language>> GetAsync(this DbSet<Language> languages, int range) =>
        await languages.Take(range).ToListAsync();

    public static async Task UpdateAsync(this DbSet<Language> languages, Language language)
    {
        if (!await languages.AnyAsync(x => x.Id == language.Id))
            throw new NotFoundException<Language>();

        languages.Update(language);
    }
}
 
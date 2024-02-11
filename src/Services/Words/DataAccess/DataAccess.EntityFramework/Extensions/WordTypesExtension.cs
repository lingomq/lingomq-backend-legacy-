using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace DataAccess.EntityFramework.Extensions;
public static class WordTypesExtension
{
    public static async Task<List<WordType>> GetAsync(this DbSet<WordType> wordTypes, int range) =>
        await wordTypes.Take(range).ToListAsync();
    public static async Task<WordType?> GetByIdAsync(this DbSet<WordType> wordTypes, Guid id) =>
        await wordTypes.FirstOrDefaultAsync(x => x.Id == id);
    public static async Task DeleteAsync(this DbSet<WordType> wordTypes, Guid id)
    {
        WordType wordType = await wordTypes.FirstAsync(x => x.Id == id);
        wordTypes.Remove(wordType);
    }
    public static async Task UpdateAsync(this DbSet<WordType> wordTypes, WordType wordType)
    {
        if (!await wordTypes.AnyAsync(x => x.Id == wordType.Id))
            throw new NotFoundException<WordType>();

        wordTypes.Update(wordType);
    }
}

using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;

namespace DataAccess.EntityFramework.Extensions;
public static class WordsExtensions
{
    public static async Task<Word?> GetByIdAsync(this DbSet<Word> words, Guid id) =>
        await words.FirstOrDefaultAsync(x => x.Id == id);
}

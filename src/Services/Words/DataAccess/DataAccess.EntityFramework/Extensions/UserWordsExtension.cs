using Microsoft.EntityFrameworkCore;
using System.Linq;
using Words.Domain.Entities;
using Words.Domain.Enums;
using Words.Domain.Models;

namespace DataAccess.EntityFramework.Extensions;
public static class UserWordsExtension
{
    public static async Task AddRepeatsAsync(this DbSet<UserWord> userWords, Guid userId, Guid wordId)
    {
        UserWord? userWord = await userWords
            .Include(x => x.User)
            .Include(x => x.Word)
            .FirstOrDefaultAsync(x => x.User!.Id == userId && x.Word!.Id == wordId);
        userWord!.Repeats += 1;
    }

    public static async Task<UserWord?> GetByIdAsync(this DbSet<UserWord> userWords, Guid id) =>
        await userWords.Include(x => x.User).Include(x => x.Word).FirstOrDefaultAsync(x => x.Id == id);

    public static async Task DeleteAsync(this DbSet<UserWord> userWords, Guid id)
    {
        UserWord userWord = await userWords.FirstAsync(x => x.Id == id);
        userWords.Remove(userWord);
    }

    public static async Task<int> GetCountWordsPerDayAsync(this DbSet<UserWord> userWords, Guid userId, DateTime date) =>
        await userWords.Include(x => x.User).Where(x => x.User!.Id == userId && x.AddedAt == date).CountAsync();

    public static async Task<List<UserWord>> GetByUserIdAsync(this DbSet<UserWord> userWords, Guid userId, int skip = 0, int take = int.MaxValue) =>
        await userWords
        .Include(x => x.User)
        .Include(x => x.Word)
        .Where(x => x.User!.Id == userId)
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    public static async Task<UserWord?> GetMostRepeatedWordAsync(this DbSet<UserWord> userWords, Guid userId)
    {
        int count = await userWords.Where(x => x.User!.Id == userId).MaxAsync(x => x.Repeats);
        return await userWords
        .Include(x => x.User)
        .Include(x => x.Word)
        .Where(x => x.Repeats == count)
        .FirstOrDefaultAsync(x => x.User!.Id == userId);
    }

    public static async Task<List<RecordsModel>> GetRecordsAsync(this DbSet<UserWord> userWords, RecordTypesEnum recordType, OrderingEnum ordering, int count)
    {
        List<RecordsModel> records = new List<RecordsModel>();
        switch (recordType)
        {
            case (RecordTypesEnum.Repeats):
            {
                    records = await userWords
                       .OrderBy(x => userWords.Sum(s => s.Repeats))
                       .GroupBy(x => x.User)
                       .Select(x => new RecordsModel { Count = userWords.Where(w => w.User!.Id == x.Key!.Id).Sum(s => s.Repeats), UserId = x.Key!.Id })
                       .Take(count)
                       .ToListAsync();
                        
                break;
            };

            case (RecordTypesEnum.Words):
                {
                    records = await userWords
                       .GroupBy(x => x.User)
                       .OrderBy(x => userWords.GroupBy(x => x.User).Count())
                       .Select(x => new RecordsModel { Count = userWords.Where(w => w.User!.Id == x.Key!.Id).Count(), UserId = x.Key!.Id })
                       .Take(count)
                       .ToListAsync();

                    break;
                }
        }

        if (ordering == OrderingEnum.DESC)
            records.Reverse();

        return records;
    }
}

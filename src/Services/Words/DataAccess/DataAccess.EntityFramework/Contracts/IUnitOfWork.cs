using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;

namespace DataAccess.EntityFramework.Contracts;
public interface IUnitOfWork
{
    public DbSet<Language> Languages { get; }
    public DbSet<User> Users { get; }
    public DbSet<UserWord> UserWords { get; }
    public DbSet<Word> Words { get; }
    public DbSet<WordType> WordTypes { get; }
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync();
}

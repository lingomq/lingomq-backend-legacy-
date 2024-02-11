using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;

namespace DataAccess.EntityFramework.Data;
public class ApplicationDbContext : DbContext
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserWord> UserWords => Set<UserWord>();
    public DbSet<Word> Words => Set<Word>();
    public DbSet<WordType> WordTypes => Set<WordType>();
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
}

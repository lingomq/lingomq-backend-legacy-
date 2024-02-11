using DataAccess.EntityFramework.Contracts;
using DataAccess.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Words.Domain.Entities;

namespace DataAccess.EntityFramework.Realizations;
public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public DbSet<Language> Languages => _context.Languages;

    public DbSet<User> Users => _context.Users;

    public DbSet<UserWord> UserWords => _context.UserWords;

    public DbSet<Word> Words => _context.Words;

    public DbSet<WordType> WordTypes => _context.WordTypes;

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

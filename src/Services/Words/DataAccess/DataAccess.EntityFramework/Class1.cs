using DataAccess.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;

namespace DataAccess.EntityFramework;
public class Class1
{
    private ApplicationDbContext _context;
    public Class1(ApplicationDbContext context)
    {
        _context = context;
    }

    public DbSet<Language> Languages => _context.Languages;
}

using Microsoft.EntityFrameworkCore;
using Words.Domain.Entities;
using Words.Domain.Exceptions.ClientExceptions;

namespace DataAccess.EntityFramework.Extensions;
public static class UsersExtension
{
    public static async Task<User?> GetByIdAsync(this DbSet<User> users, Guid id) =>
        await users.FirstOrDefaultAsync(x => x.Id == id);
    public static async Task DeleteAsync(this DbSet<User> users, Guid id)
    {
        User user = await users.FirstAsync(x => x.Id == id);
        users.Remove(user);
    }
    public static async Task UpdateAsync(this DbSet<User> users, User user)
    {
        if (!await users.AnyAsync(x => x.Id == user.Id))
            throw new NotFoundException<User>();

        users.Update(user);
    }
}

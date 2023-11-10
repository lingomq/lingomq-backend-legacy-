using Authentication.BusinessLayer.Contracts;
using Authentication.DomainLayer.Entities;

namespace Authentication.Api.Services
{
    public class DatabaseDataMigrator : IDatabaseDataMigrator
    {
        private readonly IUserRoleRepository _roleRepository;
        public DatabaseDataMigrator(IUserRoleRepository roleRepository) =>
            _roleRepository = roleRepository;

        public async Task AddRoles()
        {
            List<string> roleNames = new List<string>() { "user", "admin", "moderator" };
            var rolesFromDatabase = await _roleRepository.GetAsync(int.MaxValue);

            foreach (var role in rolesFromDatabase)
            {
                Console.WriteLine(role.Name);
                if (roleNames.Contains(role.Name!))
                    roleNames.Remove(role.Name!);
            }

            foreach (string roleName in roleNames)
                await _roleRepository.AddAsync(new UserRole() { Id = Guid.NewGuid(), Name = roleName });
        }
    }
}

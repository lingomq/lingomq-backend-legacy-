using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;

namespace Identity.Api.Services
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

            foreach (var roleName in rolesFromDatabase.Select(x => x.Name))
            {
                if (roleNames.Contains(roleName!))
                    roleNames.Remove(roleName!);
            }

            foreach (string roleName in roleNames)
                await _roleRepository.AddAsync(new UserRole() { Id = Guid.NewGuid(), Name = roleName });
        }
    }
}

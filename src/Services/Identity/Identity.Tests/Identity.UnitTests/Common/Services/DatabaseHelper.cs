using Cryptography;
using Cryptography.Cryptors;
using Cryptography.Entities;
using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using System.Data;

namespace Identity.UnitTests.Common.Services
{
    public class DatabaseHelper
    {
        public async static Task RemoveDependencies(IDbConnection connection)
        {
            connection.Open();
            await connection.ExecuteAsync("TRUNCATE TABLE users RESTART IDENTITY CASCADE;");
            await connection.ExecuteAsync("TRUNCATE TABLE user_roles RESTART IDENTITY CASCADE;");
            await connection.ExecuteAsync("TRUNCATE TABLE user_infos RESTART IDENTITY CASCADE;");
            connection.Close();
        }

        public static async Task AddRoles(IUnitOfWork unitOfWork)
        {
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "user" });
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "admin" });
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "moderator" });
        }
        public static async Task SetUserDependency(IUnitOfWork unitOfWork, UserInfo model, string email = "",
            string nickname = "", string password = "")
        {
            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(password);

            Guid userId = Guid.NewGuid();
            await unitOfWork.Users.AddAsync(new()
            {
                Id = userId,
                Email = email != "" ? email : model.User!.Email,
                Phone = model.User!.Phone,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            });

            UserRole? role = await unitOfWork.UserRoles.GetByNameAsync(model.Role!.Name!);

            await unitOfWork.UserInfos.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Nickname = nickname != "" ? nickname : model.Nickname,
                ImageUri = "uri",
                Additional = "add",
                IsRemoved = false,
                CreationalDate = DateTime.Now,
                RoleId = role!.Id,
                UserId = userId
            });
        }
    }
}

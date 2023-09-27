using Authentication.BusinessLayer.Models;
using Authentication.DomainLayer.Entities;
using Cryptography.Cryptors;
using Cryptography.Entities;
using Cryptography;
using Dapper;
using System.Data;
using Authentication.BusinessLayer.Contracts;

namespace Authentication.UnitTest.Common.Services
{
    public class DatabaseHelper
    {
        public async static Task RemoveDependencies(IDbConnection connection)
        {
            connection.Open();
            await connection.ExecuteAsync("TRUNCATE TABLE users RESTART IDENTITY CASCADE;");
            await connection.ExecuteAsync("TRUNCATE TABLE user_roles RESTART IDENTITY CASCADE;");
            await connection.ExecuteAsync("TRUNCATE TABLE user_infos RESTART IDENTITY CASCADE;");
            connection  .Close();
        }

        public static async Task AddRoles(IUnitOfWork unitOfWork)
        {
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "user" });
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "admin" });
            await unitOfWork.UserRoles.AddAsync(new() { Id = Guid.NewGuid(), Name = "moderator" });
        }
        public static async Task SetUserDependency(IUnitOfWork unitOfWork, SignInUpResponseModel model, string email = "",
            string nickname = "")
        {
            Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
            BaseKeyPair keyPair = cryptor.Crypt(model.Password!);

            Guid userId = Guid.NewGuid();
            await unitOfWork.Users.AddAsync(new()
            {
                Id = userId,
                Email = email != "" ? email : model.Email,
                Phone = model.Phone,
                PasswordHash = keyPair.Hash,
                PasswordSalt = keyPair.Salt
            });

            UserRole? role = await unitOfWork.UserRoles.GetByNameAsync("user");

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

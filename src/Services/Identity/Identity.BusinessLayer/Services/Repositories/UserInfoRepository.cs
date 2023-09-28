using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using System.Data;
using System.Transactions;

namespace Identity.BusinessLayer.Services.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly static string Get =
            "SELECT user_infos.id, " +
            "user_infos.nickname AS \"Nickname\", " +
            "user_infos.image_uri AS \"ImageUri\", " +
            "user_infos.additional AS \"Additional\", " +
            "user_infos.creational_date AS \"CreationalDate\", " +
            "user_infos.is_removed AS \"IsRemoved\", " +
            "user_roles.id, " +
            "user_roles.name AS \"Name\", " +
            "users.id, " +
            "users.email AS \"Email\", " +
            "users.phone AS \"Phone\", " +
            "users.password_hash AS \"PasswordHash\", " +
            "users.password_salt AS \"PasswordSalt\", " +
            "user_links.id, " +
            "user_links.user_info_id AS \"UserInfoId\", " +
            "user_links.link_id AS \"UserInfoId\", " +
            "FROM user_infos " +
            "LEFT JOIN user_roles ON user_infos.role_id = user_roles.id " +
            "LEFT JOIN users ON user_infos.user_id = users.id " +
            "LEFT JOIN user_links ON user_infos.user_link_id = user_links.id ";
        private readonly static string GetRange = Get +
            " LIMIT @Count";
        private readonly static string GetByNickname = Get +
            " WHERE user_infos.nickname = @Nickname";
        private readonly static string GetById = Get +
            " WHERE user_infos.id = @Id";
        private readonly static string Update =
                "UPDATE user_infos" +
                "SET nickname = @Nickname," +
                "image_uri = @Phone," +
                "additional = @PasswordHash," +
                "role_id = @PasswordSalt " +
                "user_id = @UserId " +
                "is_removed = @IsRemoved " +
                "WHERE id = @Id";
        private readonly static string Delete =
                "DELETE FROM user_infos " +
                "WHERE id = @Id";
        private readonly static string Create =
               "INSERT INTO user_infos (id, nickname, image_uri, additional, role_id, user_id, is_removed) " +
               "VALUES (@Id, @Nickname, @ImageUri, @Additional, @RoleId, @UserId, @IsRemoved);";
        private readonly IDbConnection _connection;
        public UserInfoRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<UserInfo> AddAsync(UserInfo entity)
        {
            await ExecuteAsync(entity, Create);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(Delete, new { Id = id});
            transactionScope.Complete();

            return true;
        }

        public async Task<List<UserInfo>> GetAsync(int count)
        {
            List<UserInfo> infos = await GetByTemplate(new { Count = count }, GetRange);

            return infos;
        }

        public async Task<UserInfo?> GetByIdAsync(Guid id)
        {
            List<UserInfo> infos = await GetByTemplate(new { Id = id }, GetById);

            return infos.First() is null ? null : infos.First();
        }

        public async Task<UserInfo?> GetByNicknameAsync(string nickname)
        {
            List<UserInfo> infos = await GetByTemplate(new { Nickname = nickname }, GetById);

            return infos.First() is null ? null : infos.First();
        }

        public async Task<UserInfo> UpdateAsync(UserInfo entity)
        {
            await ExecuteAsync(entity, Update);

            return entity;
        }
        private async Task<List<UserInfo>> GetByTemplate<T>(T template, string sql)
        {
            IEnumerable<UserInfo> infos;

            infos = await _connection.QueryAsync<UserInfo, UserRole, User, UserLink, UserInfo>(sql,
                (info, role, user, link) =>
                {
                    info.Role = role;
                    info.RoleId = role.Id;
                    info.User = user;
                    info.UserId = user.Id;
                    if (info.UserLink is null)
                        info.UserLink = new List<UserLink>() { link };
                    else
                        info.UserLink.Add(link);

                    info.UserLinkId = link.Id;
                    return info;
                }, template, splitOn: "id");

            return infos.ToList();
        }
        private async Task<UserInfo> ExecuteAsync<UserInfo>(UserInfo template, string sql)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(sql, template);
            transactionScope.Complete();

            return template;
        }
    }
}

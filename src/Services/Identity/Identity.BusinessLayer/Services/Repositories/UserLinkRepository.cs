using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using System.Data;
using System.Transactions;

namespace Identity.BusinessLayer.Services.Repositories
{
    public class UserLinkRepository : IUserLinkRepository
    {
        private readonly static string Get =
            "SELECT user_links.id, " +
            "user_infos.id, " +
            "user_infos.nickname as \"Nickname\", " +
            "user_infos.additional as \"Additional\", " +
            "user_infos.image_uri as \"ImageUri\", " +
            "user_infos.creational_date as \"CreationalDate\", " +
            "user_infos.is_removed as \"IsRemoved\", " +
            "users.id, " +
            "users.email as \"Email\", " +
            "users.phone as \"Phone\", " +
            "link_types.id, " +
            "link_types.name as \"LinkName\", " +
            "link_types.short_link as \"ShortLink\", " +
            "user_roles.id, " +
            "user_roles.name as \"Name\" " +
            "FROM user_links " +
            "INNER JOIN user_infos ON user_infos.id = user_links.user_infos_id " +
            "INNER JOIN users ON users.id = user_infos.user_id " +
            "INNER JOIN link_types ON link_types.id = user_links.link_id " +
            "INNER JOIN user_roles ON user_roles.id = user_infos.role_id ";
        private readonly static string GetRange = Get +
            " LIMIT @Count";
        private readonly static string GetById = Get +
            " WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO user_links (id, user_info_id, link_id) " +
            "VALUES (@Id, @UserInfoId, @LinkId)";
        private readonly static string Delete =
            "DELETE FROM user_links " +
            "WHERE id = @Id";
        private readonly static string Update =
            "UPDATE user_links" +
            "SET " +
            "user_info_id = @UserInfoId," +
            "link_id = @LinkId" +
            "WHERE id = @Id";
        private readonly IDbConnection _connection;
        public UserLinkRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task<UserLink> AddAsync(UserLink entity)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Create, entity);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Delete, new { Id = id });

            return true;
        }

        public async Task<List<UserLink>> GetAsync(int count)
        {
            IEnumerable<UserLink> links;

            links = await _connection.QueryAsync<UserLink>(GetRange, new { Count = count });

            return links.ToList();
        }

        public async Task<UserLink?> GetByIdAsync(Guid id)
        {
            IEnumerable<UserLink> links;

            links = await _connection.QueryAsync<UserLink>(GetById, new { Id = id });

            return links.FirstOrDefault() is null ? null : links.FirstOrDefault();
        }

        public async Task<UserLink> UpdateAsync(UserLink entity)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Update, entity);

            return entity;
        }
    }
}

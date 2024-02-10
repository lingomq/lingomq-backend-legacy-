using Dapper;
using Finances.BusinessLayer.Contracts;
using Finances.DomainLayer.Entities;
using System.Data;

namespace Finances.BusinessLayer.Services.Repositories;

public class UserFinanceRepository : GenericRepository<UserFinance>, IUserFinanceRepository
{
    private static readonly string Get = "SELECT user_finances.id, " +
                                         "creation_date as \"CreationDate\"," +
                                         "end_subscription_date as \"EndSubscriptionDate\"," +
                                         "users.id," +
                                         "email as \"Email\"," +
                                         "phone as \"Phone\"," +
                                         "finances.id," +
                                         "name as \"FinanceDate\"," +
                                         "description as \"Description\"," +
                                         "amount as \"Amount\" " +
                                         "FROM user_finances " +
                                         "JOIN users ON users.id = user_finances.user_id " +
                                         "JOIN finances ON finances.id = user_finances.finance_id ";

    private static readonly string GetRange = Get + "LIMIT @Count";
    private static readonly string GetById = Get + "WHERE id = @Id";
    private static readonly string GetByUserId = Get + "WHERE users.id = @Id";

    private static readonly string Create = "INSERT INTO user_finances " +
                                            "(id, user_id, finances_id, creation_date, end_subscription_date) " +
                                            "VALUES " +
                                            "(@Id, @UserId, @FinancesId, @CreationDate, @EndSubscriptionDate)";

    private static readonly string Update = "UPDATE user_finances SET " +
                                            "user_id = @UserId," +
                                            "finances_id = @FinancesId," +
                                            "creation_date = @CreationDate," +
                                            "end_subscription_date = @EndSubscriptionDate " +
                                            "WHERE id = @Id";

    private static readonly string Delete = "DELETE FROM user_finances " +
                                            "WHERE id = @Id";

    private readonly IDbConnection _connection;
    public UserFinanceRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }

    public async Task<UserFinance?> GetAsync(Guid id)
    {
        List<UserFinance> userFinances = await QueryAsync(GetById, new { Id = id });
        return userFinances.FirstOrDefault();
    }

    public async Task<List<UserFinance>> GetAsync(int range) =>
        await QueryAsync(GetRange, new { Count = range });

    public async Task CreateAsync(UserFinance entity) =>
        await ExecuteAsync(Create, entity);

    public async Task UpdateAsync(UserFinance entity) =>
        await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteAsync(Delete, new { Id = id });

    public async Task<List<UserFinance>> GetByUserIdAsync(Guid userId) =>
        await QueryAsync(GetByUserId, new { Id = userId });

    protected override async Task<List<UserFinance>> QueryAsync<TE>(string sql, TE entity)
    {
        IEnumerable<UserFinance> values = await _connection
            .QueryAsync<UserFinance, User, Finance, UserFinance>(sql,
                (userFinance, user, finance) =>
                {
                    userFinance.Finance = finance;
                    userFinance.User = user;
                    userFinance.FinanceId = finance.Id;
                    userFinance.UserId = user.Id;

                    return userFinance;
                }, entity, splitOn: "id");

        return values.ToList();
    }
}
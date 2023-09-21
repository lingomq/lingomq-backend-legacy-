using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Dtos;
using Authentication.DomainLayer.Entities;
using AutoMapper;
using Dapper;
using System.Data;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Authentication.BusinessLayer.Services.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly IDbConnection _connection;
        private readonly IMapper _mapper;
        public UserInfoRepository(IDbConnection configuration, IMapper mapper)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = configuration;
            _mapper = mapper;
        }
        public async Task<UserInfoDto> AddAsync(UserInfo entity)
        {
            string sql =
               "INSERT INTO user_infos (id, nickname, image_uri, additional, role_id, user_id, is_removed) " +
               "VALUES (@Id, @Nickname, @ImageUri, @Additional, @RoleId, @UserId, @IsRemoved);";

            using var transactionScope = new TransactionScope();

            try
            {
                int result = await _connection.ExecuteAsync(sql, entity);
                transactionScope.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }

            UserInfoDto infoDto = _mapper.Map<UserInfo, UserInfoDto>(entity);

            return infoDto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            string sql =
                "DELETE FROM user_infos " +
                "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            try
            {
                int result = await _connection.ExecuteAsync(sql, new { Id = id });

                transactionScope.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }

        public async Task<List<UserInfoDto>?> GetAsync(int count = 0)
        {
            IEnumerable<UserInfo> users;
            if (count > 0)
                users = await _connection
                    .QueryAsync<UserInfo, User, UserRole, UserInfo>(
                    "SELECT id, nickname, image_uri, additional, creational_date, i.role_id, i.user_id, is_removed " +
                    "FROM user_infos i " +
                    "INNER JOIN users u ON i.user_id = u.id " +
                    "INNER JOIN user_roles r ON i.role_id = r.id " +
                    "TAKE @Count;", (userInfo, user, role) =>
                    {
                        userInfo.Role = role;
                        userInfo.User = user;
                        return userInfo;
                    },
                    new { Count = count });
            else
                users = await _connection.QueryAsync<UserInfo>("SELECT * FROM user_infos;");

            List<UserInfoDto> userViews =
                _mapper.Map<List<UserInfo>, List<UserInfoDto>>(users.ToList());

            return userViews.Count > 0 ? userViews : null;
        }

        public async Task<UserInfoDto?> GetByGuidAsync(Guid guid)
        {
            IEnumerable<UserInfo> userInfos = await _connection
                .QueryAsync<UserInfo, User, UserRole, UserInfo>(
                    "SELECT id, nickname, image_uri, additional, creational_date, i.role_id, i.user_id, is_removed " +
                    "FROM user_infos i " +
                    "INNER JOIN users u ON i.user_id = u.id " +
                    "INNER JOIN user_roles r ON i.role_id = r.id " +
                    "WHERE id = @Id;", (userInfo, user, role) =>
                    {
                        userInfo.Role = role;
                        userInfo.User = user;
                        return userInfo;
                    },
                    new { Id = guid });

            if (userInfos.Count() < 0)
                return null;

            UserInfoDto infoDto = _mapper.Map<UserInfo, UserInfoDto>(userInfos.First());

            return infoDto;
        }

        public async Task<UserInfoDto> UpdateAsync(UserInfo entity)
        {
            string sql = 
                "UPDATE user_infos" +
                "SET nickname = @Nickname," +
                "image_uri = @Phone," +
                "additional = @PasswordHash," +
                "role_id = @PasswordSalt " +
                "user_id = @UserId " +
                "is_removed = @IsRemoved " +
                "WHERE id = @Id";

            using var transactionScope = new TransactionScope();

            try
            {
                int result = await _connection.ExecuteAsync(sql, entity);
                transactionScope.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }

            UserInfoDto infoDto = _mapper.Map<UserInfo, UserInfoDto>(entity);

            return infoDto;
        }
    }
}

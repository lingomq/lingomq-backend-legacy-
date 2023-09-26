using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Dtos;
using Authentication.DomainLayer.Entities;
using AutoMapper;
using Dapper;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Transactions;

namespace Authentication.BusinessLayer.Services.Repositories
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
            "users.phone AS \"Phone\" " +
            "FROM user_infos " +
            "LEFT JOIN user_roles ON user_infos.role_id = user_roles.id " +
            "LEFT JOIN users ON user_infos.user_id = users.id ";
        private readonly static string GetRange = Get +
            " TAKE @Count";
        private readonly static string GetByNickname = Get +
            " WHERE user_infos.nickname = @Nickname;";
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
        private readonly IMapper _mapper;
        public UserInfoRepository(IDbConnection configuration, IMapper mapper)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = configuration;
            _mapper = mapper;

        }
        public async Task<UserInfoDto> AddAsync(UserInfo entity)
        {
            return await ExecuteByTemplate(entity, Create);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await ExecuteByTemplate(new { Id = id }, Delete);
            return true;
        }

        public async Task<List<UserInfoDto>> GetAsync(int count = int.MaxValue)
        {
            IEnumerable<UserInfo> users;
            users = await _connection.QueryAsync<UserInfo, UserRole, User, UserInfo>(GetRange,
                (userInfo, role, user) =>
                {
                    userInfo.Role = role;
                    userInfo.RoleId = role.Id;
                    userInfo.User = user;
                    userInfo.UserId = user.Id;
                    return userInfo;
                },
                new { Count = count }, splitOn: "id");

            List<UserInfoDto> userViews =
                _mapper.Map<List<UserInfoDto>>(users.ToList());

            return userViews;
        }

        public async Task<UserInfoDto?> GetByNicknameAsync(string nickname)
        {
            return await GetByTemplate(new { Nickname = nickname }, GetByNickname);
        }

        public async Task<UserInfoDto?> GetByGuidAsync(Guid guid)
        {
            return await GetByTemplate(new { Id = guid }, GetById);
        }

        public async Task<UserInfoDto> UpdateAsync(UserInfo entity)
        {
            return await ExecuteByTemplate(entity, Update);
        }
        private async Task<UserInfoDto?> GetByTemplate<T>(T template, string sql) where T : class
        {
            IEnumerable<UserInfo> userInfos = await _connection
                .QueryAsync<UserInfo, UserRole, User, UserInfo>(sql,
                (userInfo, role, user) =>
                {
                    userInfo.Role = role;
                    userInfo.RoleId = role.Id;
                    userInfo.User = user;
                    userInfo.UserId = user.Id;
                    return userInfo;
                },
                template, splitOn: "id");

            if (userInfos.Count() < 1)
                return null;

            UserInfoDto infoDto = _mapper.Map<UserInfoDto>(userInfos.First());

            return infoDto;
        }
        private async Task<UserInfoDto> ExecuteByTemplate<T>(T template, string sql) where T : class 
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            int result = await _connection.ExecuteAsync(sql, template);
            transactionScope.Complete();

            UserInfoDto infoDto = _mapper.Map<UserInfoDto>(template);

            return infoDto;
        }
        private string GetDescriptionFromAttribute(MemberInfo member)
        {
            if (member == null) return null;

            var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);
            return (attrib?.Description ?? member.Name).ToLower();
        }
    }
}

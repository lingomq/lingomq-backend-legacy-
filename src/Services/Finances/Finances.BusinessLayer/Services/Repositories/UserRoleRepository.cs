﻿using Dapper;
using Finances.BusinessLayer.Contracts;
using Finances.DomainLayer.Entities;
using System.Data;
using System.Transactions;

namespace Finances.BusinessLayer.Services.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private static readonly string Get =
            "SELECT id as \"Id\", name as \"Name\" " +
            "FROM user_roles";
        private static readonly string GetById = Get +
            " WHERE id = @Id";
        private static readonly string GetByName = Get +
            " WHERE name = @Name";
        private static readonly string GetRange = Get +
            " LIMIT @Count";
        private static readonly string Create =
            "INSERT INTO user_roles " +
            "(id, name) " +
            "VALUES " +
            "(@Id, @Name);";
        private static readonly string Delete =
            "DELETE FROM user_roles " +
            "WHERE id = @Id";
        private static readonly string Update =
            "UPDATE user_roles " +
            "SET name = @Name " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public UserRoleRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task CreateAsync(UserRole entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();
        }

        public async Task<List<UserRole>> GetAsync(int count = int.MaxValue)
        {
            IEnumerable<UserRole> roles;

            roles = await _connection.QueryAsync<UserRole>(GetRange, new { Count = count });
            return roles.ToList() is null ? new List<UserRole>() : roles.ToList();
        }

        public async Task<UserRole?> GetAsync(Guid id)
        {
            IEnumerable<UserRole> roles;

            roles = await _connection.QueryAsync<UserRole>(GetById, new { Id = id });

            return roles.FirstOrDefault() is not null ? roles.FirstOrDefault() : null;
        }

        public async Task<UserRole?> GetByNameAsync(string name)
        {
            IEnumerable<UserRole> roles;

            roles = await _connection.QueryAsync<UserRole>(GetByName, new { Name = name });

            return roles.FirstOrDefault() is not null ? roles.FirstOrDefault() : null;
        }

        public async Task UpdateAsync(UserRole entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();
        }
    }
}

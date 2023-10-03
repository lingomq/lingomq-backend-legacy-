﻿using Dapper;
using Identity.BusinessLayer.Contracts;
using Identity.DomainLayer.Entities;
using System.Data;
using System.Transactions;

namespace Identity.BusinessLayer.Services.Repositories
{
    public class LinkTypeRepository : ILinkTypeRepository
    {
        private static readonly string Get =
            "SELECT id as \"Id\", name as \"LinkName\", short_link as \"ShortLink\" " +
            "FROM link_types ";
        private static readonly string GetRange = Get +
            "LIMIT @Count";
        private static readonly string GetById = Get +
            "WHERE id = @Id";
        private static readonly string GetByLinkName = Get +
            "WHERE name = @LinkName";
        private static readonly string Create =
            "INSERT INTO link_types " +
            "(id, name, short_name) " +
            "VALUES (@Id, @LinkName, @ShortName)";
        private static readonly string Update =
            "UPDATE link_types " +
            "SET name = @LinkName, short_link = @ShortLink " +
            "WHERE id = @Id";
        private static readonly string Delete =
            "DELETE FROM link_types " +
            "WHERE id = @Id";

        private readonly IDbConnection _connection;
        public LinkTypeRepository(IDbConnection connection) =>
            _connection = connection;
        public async Task<LinkType> AddAsync(LinkType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope();

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();

            return true;
        }

        public async Task<List<LinkType>> GetAsync(int count = int.MaxValue)
        {
            IEnumerable<LinkType> types;

            types = await _connection.QueryAsync<LinkType>(GetRange, new { Count = count });

            return types.ToList() is null ? new List<LinkType>() : types.ToList();
        }

        public async Task<LinkType?> GetByIdAsync(Guid id)
        {
            IEnumerable<LinkType> types;

            types = await _connection.QueryAsync<LinkType>(GetById, new { Id = id });

            return types.FirstOrDefault() is null ? null : types.FirstOrDefault();
        }

        public async Task<LinkType?> GetByNameAsync(string name)
        {
            IEnumerable<LinkType> types;

            types = await _connection.QueryAsync<LinkType>(GetByLinkName, new { LinkName = name });

            return types.FirstOrDefault() is null ? null : types.FirstOrDefault();
        }

        public async Task<LinkType> UpdateAsync(LinkType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}
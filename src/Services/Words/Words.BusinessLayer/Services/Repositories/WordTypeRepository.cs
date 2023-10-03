using Dapper;
using System;
using System.Data;
using System.Transactions;
using Words.BusinessLayer.Contracts;
using Words.DomainLayer.Entities;
using static Dapper.SqlMapper;

namespace Words.BusinessLayer.Services.Repositories
{
    public class WordTypeRepository : IWordTypeRepository
    {
        private readonly static string Get =
            "SELECT id as \"Id\", type_name as \"TypeName\" " +
            "FROM word_types ";
        private readonly static string GetRange =
            "LIMIT @Count";
        private readonly static string GetById =
            "WHERE id = @Id";
        private readonly static string Create =
            "INSERT INTO word_types (id, type_name) " +
            "VALUES (@Id, @TypeName)";
        private readonly static string Update =
            "UPDATE word_types " +
            "SET type_name = @TypeName " +
            "WHERE id = @ id";
        private readonly static string Delete =
            "DELETE FROM word_types WHERE id = @Id";
        private readonly IDbConnection _connection;
        public WordTypeRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<WordType> AddAsync(WordType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Create, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Delete, new { Id = id });
            transactionScope.Complete();
            transactionScope.Dispose();

            return true;
        }

        public async Task<List<WordType>> GetAsync(int range = int.MaxValue)
        {
            IEnumerable<WordType> types;
            types = await _connection.QueryAsync<WordType>(GetRange, new { Count = range });

            return types.Count() < 1 ? new List<WordType>() : types.ToList();
        }

        public async Task<WordType?> GetByIdAsync(Guid id)
        {
            IEnumerable<WordType> types;
            types = await _connection.QueryAsync<WordType>(GetById, new { Id = id });

            return types.FirstOrDefault() is null ? null : types.FirstOrDefault();
        }

        public async Task<WordType> UpdateAsync(WordType entity)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _connection.ExecuteAsync(Update, entity);
            transactionScope.Complete();
            transactionScope.Dispose();

            return entity;
        }
    }
}

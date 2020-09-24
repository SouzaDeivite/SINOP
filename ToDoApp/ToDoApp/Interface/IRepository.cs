// importações das bibliotecas
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Remove(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task Remove(int id, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task RemoveList(List<TEntity> lstEntity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task<TEntity> Create(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task<List<TEntity>> CreateList(List<TEntity> lstEntity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task Insert(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task Update(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        //  public Task<TEntity> GetById(object id, SqlConnection conexao = null);
        Task<List<TEntity>> GetAll(SqlConnection conexao = null);
        Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> predicate);
        Task<int> Execute(string Sql, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null);
        Task<TEntity> SetValue(TEntity entity);
        Task<TEntity> SetValue(TEntity entity, string field, decimal valor);
        Task<List<TEntity>> SetValueList(List<TEntity> lstEntity, string field, decimal valor);
    }
}
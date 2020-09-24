// importações das bibliotecas
using System;
using Dapper;
using System.Linq;
using ToDoApp.Interface;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
// name space do projeto
namespace ToDoApp.Base
{
    public class BaseRepository<TEntity> : AbstractRepository, IRepository<TEntity> where TEntity : class, new()
    {
        readonly IDbContext _context;

        public BaseRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
            _context = context;
        }

        public Task Remove(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Delete(entity, transaction: transacaoSqlBanco));
        }

        public Task Remove(int id, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            if (conexao == null)
                return Task.FromResult(_context.Connection.Delete(_context.Connection.Get<TEntity>(id, transaction: transacaoSqlBanco)));
            else
                return Task.FromResult(conexao.Delete(conexao.Get<TEntity>(id, transaction: transacaoSqlBanco)));
        }

        public Task RemoveList(List<TEntity> lstEntity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Delete(lstEntity, transaction: transacaoSqlBanco));
        }

        public Task<List<TEntity>> GetAll(SqlConnection conexao = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).GetAll<TEntity>().ToList());
        }

        public Task<TEntity> GetById(object id, SqlConnection conexao = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Get<TEntity>(id));
        }

        public Task<TEntity> Create(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            var keys = FindExplicitKeys(typeof(TEntity));

            if (keys.Count() == 1)
            {
                var key = keys.FirstOrDefault();

                var value = NextKey<TEntity>(conexao);

                key.SetValue(entity, value);
            }

            (conexao == null ? _context.Connection : conexao).Insert(entity, transaction: transacaoSqlBanco);

            return Task.FromResult(entity);
        }

        public Task Insert(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Insert(entity, transaction: transacaoSqlBanco));
        }

        public Task<List<TEntity>> CreateList(List<TEntity> lstEntity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            Type type = lstEntity.GetType().GetGenericArguments()[0];

            var keys = FindExplicitKeys(type);

            var keyField = "";
            object valueField = 0;

            if (keys.Count() == 1)
            {
                var key = keys.FirstOrDefault();
                var value = NextKey<TEntity>(conexao);

                keyField = key.Name;
                valueField = value;
            }

            lstEntity.ForEach(e =>
            {
                var nameOfProperty = keyField;
                var propertyInfo = e.GetType().GetProperty(nameOfProperty);
                var valueKey = propertyInfo.GetValue(e, null);
                if ((decimal)valueKey == 0)
                    propertyInfo.SetValue(e, valueField);
            });

            Task.FromResult((conexao == null ? _context.Connection : conexao).Insert(lstEntity, transaction: transacaoSqlBanco));

            return Task.FromResult(lstEntity);
        }

        public Task Update(TEntity entity, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Update(entity, transaction: transacaoSqlBanco));
        }

        public Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(_context.Connection.GetAll<TEntity>().Where(predicate));
        }

        public Task<int> Execute(string Sql, SqlConnection conexao = null, SqlTransaction transacaoSqlBanco = null)
        {
            return Task.FromResult((conexao == null ? _context.Connection : conexao).Execute(Sql, transaction: transacaoSqlBanco));
        }

        public Task<TEntity> SetValue(TEntity entity, string field, decimal valor)
        {
            var propertyInfo = entity.GetType().GetProperty(field);
            var valueKey = propertyInfo.GetValue(entity, null);
            if ((decimal)valueKey == 0)
                propertyInfo.SetValue(entity, Convert.ToDecimal(valor));
            return Task.FromResult(entity);
        }

        public Task<List<TEntity>> SetValueList(List<TEntity> lstEntity, string field, decimal valor)
        {
            lstEntity.ForEach(e =>
            {
                var propertyInfo = e.GetType().GetProperty(field);
                var valueKey = propertyInfo.GetValue(e, null);
                if ((decimal)valueKey == 0)
                    propertyInfo.SetValue(e, Convert.ToDecimal(valor));

            });
            return Task.FromResult(lstEntity);
        }

        public Task<TEntity> SetValue(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
// importações das bibliotecas
using System;
using Dapper;
using System.Linq;
using System.Reflection;
using ToDoApp.Interface;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
// name space do projeto
namespace ToDoApp.Base
{
    public abstract class AbstractRepository
    {
        private readonly IConfiguration _config;
        readonly IDbContext _context;

        protected AbstractRepository(IConfiguration config, IDbContext context)
        {
            _config = config;
            _context = context;
        }

        protected string ConnectionString => _config.GetConnectionString("ToDoConnection");

        protected IEnumerable<PropertyInfo> FindExplicitKeys(Type type)
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes(typeof(ExplicitKeyAttribute), true).Length > 0);
        }

        protected object NextKey<TEntity>(SqlConnection conexao, SqlTransaction transacao) where TEntity : class
        {
            var key = FindExplicitKeys(typeof(TEntity)).FirstOrDefault();

            if (key.PropertyType.Equals(typeof(Guid)))
                return Guid.NewGuid();
            else
                return (conexao == null ? _context.Connection : conexao).GetAll<TEntity>(transacao).Select(r => (decimal)key.GetValue(r)).OrderBy(r => r).LastOrDefault() + 1;
        }

        protected object NextKey<TEntity>(SqlConnection conexao = null) where TEntity : class
        {
            return NextKey<TEntity>(conexao, null);
        }

        protected int NextKeyTable(string tableName, string fieldName)
        {
            return NextKeyTable(null, tableName, fieldName);
        }

        protected int NextKeyTable(SqlConnection conexao, string tableName, string fieldName)
        {
            string sql = $"Select (IsNull(Max({fieldName}), 0) + 1) as ProximoCodigo From {tableName} with(nolock) Where {fieldName} > 0";

            return (conexao == null ? _context.Connection : conexao).Query<int>(sql).SingleOrDefault();
        }
    }
}
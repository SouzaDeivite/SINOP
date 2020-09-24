// importações das bibliotecas
using System.Data;
using ToDoApp.Interface;
using System.Transactions;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// name space do projeto
namespace ToDoApp.Base
{
    public class DbContext : IDbContext
    {
        readonly IConfiguration _config;
        public IDbConnection Connection { get; set; }

        public DbContext(IConfiguration config)
        {
            _config = config;
            Connection = new SqlConnection(_config.GetConnectionString("TodoConnection"));
            Connection.Open();
        }

        public TransactionScope OpenConnectionScopeTransaction()
        {
            TransactionScope scope = new TransactionScope();
            Connection = new SqlConnection(_config.GetConnectionString("TodoConnection"));
            Connection.Open();
            return scope;
        }

        public TransactionScope OpenConnectionScopeTransactionAsync()
        {
            TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            Connection = new SqlConnection(_config.GetConnectionString("TodoConnection"));
            Connection.Open();
            return scope;
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
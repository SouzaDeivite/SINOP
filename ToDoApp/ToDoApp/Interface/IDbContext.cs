// importações das bibliotecas
using System;
using System.Data;
using System.Transactions;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }
        TransactionScope OpenConnectionScopeTransaction();
        TransactionScope OpenConnectionScopeTransactionAsync();
    }
}
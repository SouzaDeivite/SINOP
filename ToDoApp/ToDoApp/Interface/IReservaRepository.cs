// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IReservaRepository : IRepository<ReservaModel>
    {
        Task<ReservaModel> RecuperarReserva(decimal codigoReserva);
        Task<List<ReservaModel>> RecuperarReservas();
        Task<List<ReservaModel>> RecuperarReservaPorEmprestimo(decimal codigoEmprestimo);
        Task<ResultadoExecucaoQuery<decimal>> IncluirReserva(ReservaModel reservaModel);
        Task<ResultadoExecucaoQuery> AlterarReserva(ReservaModel reservaModel);
        Task<ResultadoExecucaoQuery> InativacaoReserva(ReservaModel reservaModel);
        Task<ResultadoExecucaoQuery> RemoveReserva(decimal codigoEmprestimo);
        Task<ResultadoExecucaoQuery> IncluirLivroReserva(decimal codigoEmprestimo);
    }
}
// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IEmprestimoRepository : IRepository<EmprestimoModel>
    {
        Task<EmprestimoModel> RecuperarEmprestimo(decimal codigoEmprestimo);
        Task<List<EmprestimoModel>> RecuperarEmprestimos();
        Task<List<EmprestimoModel>> RecuperarEmprestimosPorDevolucao(int devolucao);
        Task<int> ValidaUsuarioEmpresteMaximo(decimal codigoUsuario);
        Task<List<EmprestimoModel>> ValidaUsuarioEmprestimoExtrapolado();
        Task<ResultadoExecucaoQuery<decimal>> IncluirEmprestimo(EmprestimoModel emprestimoModel);
        Task<ResultadoExecucaoQuery> AlterarEmprestimo(EmprestimoModel emprestimoModel);
        Task<ResultadoExecucaoQuery> DevolvidoEmprestimo(decimal codigoEmprestimo);
    }
}
// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IInstituicaoEnsinoRepository : IRepository<InstituicaoEnsinoModel>
    {
        Task<InstituicaoEnsinoModel> RecuperarInstituicaoEnsino(decimal InstituicaoEnsino);
        Task<List<InstituicaoEnsinoModel>> RecuperarInstituicoesEnsino();
        Task<List<InstituicaoEnsinoModel>> RecuperarInstituicoesEnsinoAtivos();
        Task<ResultadoExecucaoQuery<decimal>> IncluirInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel);
        Task<ResultadoExecucaoQuery> AlterarInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel);
        Task<ResultadoExecucaoQuery> InativacaoInstituicaoEnsino(decimal codigoInstituicaoEnsino);
    }
}
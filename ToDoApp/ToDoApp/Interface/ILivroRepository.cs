// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface ILivroRepository : IRepository<LivroModel>
    {
        Task<LivroModel> RecuperarLivro(decimal codigoLivro);
        Task<List<LivroModel>> RecuperarLivros();
        Task<List<LivroModel>> RecuperarLivrosDisponiveisEmprestimo();
        Task<ResultadoExecucaoQuery<decimal>> IncluirLivro(LivroModel livroModel);
        Task<ResultadoExecucaoQuery> AlterarLivro(LivroModel livroModel);
        Task<ResultadoExecucaoQuery> InativacaoLivro(decimal codigoLivro);
    }
}
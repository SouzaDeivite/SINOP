// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Interface
{
    public interface IUsuarioRepository : IRepository<UsuarioModel>
    {
        Task<UsuarioModel> RecuperarUsuario(decimal codigoUsuario);
        Task<List<UsuarioModel>> RecuperarUsuarios();
        Task<List<UsuarioModel>> RecuperarUsuariosDisponiveisEmprestimo();
        Task<ResultadoExecucaoQuery<decimal>> IncluirUsuario(UsuarioModel usuarioModel);
        Task<ResultadoExecucaoQuery> AlterarUsuario(UsuarioModel usuarioModel);
        Task<ResultadoExecucaoQuery> InativacaoUsuario(decimal codigoUsuraio);
    }
}
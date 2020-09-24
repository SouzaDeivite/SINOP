// importações das bibliotecas
using System.Linq;
using ToDoApp.Model;
using ToDoApp.Class;
using ToDoApp.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// name space do projeto
namespace ToDoApp.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : MainController
    {
        IUsuarioRepository _usuarioRepository = null;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Método para recuperar o usuário pelo código.
        /// </summary>
        /// <param name="codigoUsuario">informar o código do usuário [decimal]</param>
        /// <returns>Objeto Usuário</returns>
        /// <response code="200">Objeto do tipo de usuário especifico.</response>
        [HttpGet("RecuperarUsuario")]
        public async Task<ActionResult> RecuperarUsuario(decimal codigoUsuario)
        {
            return Ok(await _usuarioRepository.RecuperarUsuario(codigoUsuario));
        }

        /// <summary>
        /// Método para recuperar uma lista de usuários.
        /// </summary>
        /// <returns>Lista usuários</returns>
        /// <response code="200">Lista do tipo de usuários.</response>
        [HttpGet("RecuperarUsuarios")]
        public async Task<ActionResult> RecuperarUsuarios()
        {
            DataSourceResult<UsuarioModel> rExecucao = new DataSourceResult<UsuarioModel>();
            rExecucao.Data = _usuarioRepository.RecuperarUsuarios().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para recuperar uma lista de usuários disponíveis para emrepstimos.
        /// </summary>
        /// <returns>Lista usuários</returns>
        /// <response code="200">Lista do tipo de usuários.</response>
        [HttpGet("RecuperarUsuariosDisponiveisEmprestimo")]
        public async Task<ActionResult> RecuperarUsuariosDisponiveisEmprestimo()
        {
            DataSourceResult<UsuarioModel> rExecucao = new DataSourceResult<UsuarioModel>();
            rExecucao.Data = _usuarioRepository.RecuperarUsuariosDisponiveisEmprestimo().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para incluir um usuários.
        /// </summary>
        /// <param name="usuarioModel">Informa o objeto do tipo usuário. [UsuarioModel]</param>
        /// <returns>Objeto Resposta</returns>
        /// <response code="200">O código incluído na base de dados.</response>
        [HttpPost("IncluirUsuario")]
        public async Task<ActionResult> IncluirUsuario(UsuarioModel usuarioModel)
        {
            return Ok(await _usuarioRepository.IncluirUsuario(usuarioModel));
        }

        /// <summary>
        /// Método para alterar o usuários.
        /// </summary>
        /// <param name="usuarioModel">Informa o objeto do tipo usuário. [UsuarioModel]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("AlterarUsuario")]
        public IActionResult AlterarUsuario(UsuarioModel usuarioModel)
        {
            _usuarioRepository.AlterarUsuario(usuarioModel);
            return Ok();
        }

        /// <summary>
        /// Método para inativar/desativar o usuários.
        /// </summary>
        /// <param name="codigoUsuario">Informa o código do usuário. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("InativacaoUsuario")]
        public IActionResult InativacaoUsuario(decimal codigoUsuario)
        {
            _usuarioRepository.InativacaoUsuario(codigoUsuario);
            return Ok();
        }

    }
}
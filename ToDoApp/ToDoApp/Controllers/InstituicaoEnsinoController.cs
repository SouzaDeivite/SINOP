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
    public class InstituicaoEnsinoController : MainController
    {
        IInstituicaoEnsinoRepository _instituicaoEnsinoRepository = null;

        public InstituicaoEnsinoController(IInstituicaoEnsinoRepository instituicaoEnsinoRepository)
        {
            _instituicaoEnsinoRepository = instituicaoEnsinoRepository;
        }

        /// <summary>
        /// Método para recuperar a instituição de ensino pelo código.
        /// </summary>
        /// <param name="codigoInstituicaoEnsino">informar o código da instituição de ensino [decimal]</param>
        /// <returns>Objeto Instituição de ensino</returns>
        /// <response code="200">Objeto do tipo de Instituição de Ensino especifico.</response>
        [HttpGet("RecuperarInstituicaoEnsino")]
        public async Task<ActionResult> RecuperarInstituicaoEnsino(decimal codigoInstituicaoEnsino)
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return Ok(await _instituicaoEnsinoRepository.RecuperarInstituicaoEnsino(codigoInstituicaoEnsino));
        }

        /// <summary>
        /// Método para recuperar uma lista de instituição de ensino.
        /// </summary>
        /// <returns>Objeto Instituição de ensino</returns>
        /// <response code="200">Lista do tipo de Instituição de Ensino especifico.</response>
        [HttpGet("RecuperarInstituicoesEnsino")]
        public async Task<ActionResult> RecuperarInstituicoesEnsino()
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            DataSourceResult<InstituicaoEnsinoModel> rExecucao = new DataSourceResult<InstituicaoEnsinoModel>();
            rExecucao.Data = _instituicaoEnsinoRepository.RecuperarInstituicoesEnsino().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para recuperar uma lista de instituição de ensino ativas.
        /// </summary>
        /// <returns>Objeto Instituição de ensino</returns>
        /// <response code="200">Lista do tipo de Instituição de Ensino especifico.</response>
        [HttpGet("RecuperarInstituicoesEnsinoAtivos")]
        public async Task<ActionResult> RecuperarInstituicoesEnsinoAtivos()
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            DataSourceResult<InstituicaoEnsinoModel> rExecucao = new DataSourceResult<InstituicaoEnsinoModel>();
            rExecucao.Data = _instituicaoEnsinoRepository.RecuperarInstituicoesEnsinoAtivos().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para incluir uma instituição de ensino.
        /// </summary>
        /// <param name="instituicaoEnsinoModel">Informa o objeto do tipo instituição de ensino. [InstituicaoEnsinoModel]</param>
        /// <returns>Objeto Resposta</returns>
        /// <response code="200">O código incluído na base de dados.</response>
        [HttpPost("IncluirInstituicaoEnsino")]
        public async Task<ActionResult> IncluirInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel)
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return Ok(await _instituicaoEnsinoRepository.IncluirInstituicaoEnsino(instituicaoEnsinoModel));
        }

        /// <summary>
        /// Método para alterar a instituição de ensino.
        /// </summary>
        /// <param name="instituicaoEnsinoModel">Informa o objeto do tipo instituição de ensino. [InstituicaoEnsinoModel]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("AlterarInstituicaoEnsino")]
        public IActionResult AlterarInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel)
        {
            _instituicaoEnsinoRepository.AlterarInstituicaoEnsino(instituicaoEnsinoModel);
            return Ok();
        }

        /// <summary>
        /// Método para inativar/desativar a instituição de ensino.
        /// </summary>
        /// <param name="codigoInstituicaoEnsino">Informa o código da instituição de ensino. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("InativacaoInstituicaoEnsino")]
        public IActionResult InativacaoLivro(decimal codigoInstituicaoEnsino)
        {
            _instituicaoEnsinoRepository.InativacaoInstituicaoEnsino(codigoInstituicaoEnsino);
            return Ok();
        }

    }
}
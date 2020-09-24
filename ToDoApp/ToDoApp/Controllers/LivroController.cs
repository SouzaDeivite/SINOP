// importações das bibliotecas
using System.Linq;
using ToDoApp.Class;
using ToDoApp.Model;
using ToDoApp.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// name space do projeto
namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : MainController
    {
        ILivroRepository _livroRepository = null;

        public LivroController(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        /// <summary>
        /// Método para recuperar o livro pelo código.
        /// </summary>
        /// <param name="codigoLivro">informar o código do livro [decimal]</param>
        /// <returns>Objeto Livro</returns>
        /// <response code="200">Objeto do tipo de livro especifico.</response>
        [HttpGet("RecuperarLivro")]
        public async Task<ActionResult> RecuperarLivro(decimal codigoLivro)
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return Ok(await _livroRepository.RecuperarLivro(codigoLivro));
        }

        /// <summary>
        /// Método para recuperar uma lista de livros.
        /// </summary>
        /// <returns>Lista livros</returns>
        /// <response code="200">Lista do tipo de livros.</response>
        [HttpGet("RecuperarLivros")]
        public async Task<ActionResult> RecuperarLivros()
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            DataSourceResult<LivroModel> rExecucao = new DataSourceResult<LivroModel>();
            rExecucao.Data = _livroRepository.RecuperarLivros().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para recuperar uma lista de livros disponíveis para emrepstimos.
        /// </summary>
        /// <returns>Lista livros</returns>
        /// <response code="200">Lista do tipo de livros.</response>
        [HttpGet("RecuperarLivrosDisponiveisEmprestimo")]
        public async Task<ActionResult> RecuperarLivrosDisponiveisEmprestimo()
        {
            this.ControllerContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            DataSourceResult<LivroModel> rExecucao = new DataSourceResult<LivroModel>();
            rExecucao.Data = _livroRepository.RecuperarLivrosDisponiveisEmprestimo().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para incluir um livro.
        /// </summary>
        /// <param name="livroModel">Informa o objeto do tipo livro. [LivroModel]</param>
        /// <returns>Objeto Resposta</returns>
        /// <response code="200">O código incluído na base de dados.</response>
        [HttpPost("IncluirLivro")]
        public async Task<ActionResult> IncluirLivro(LivroModel livroModel)
        {
            return Ok(await _livroRepository.IncluirLivro(livroModel));
        }

        /// <summary>
        /// Método para alterar o livro.
        /// </summary>
        /// <param name="livroModel">Informa o objeto do tipo livro. [LivroModel]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("AlterarLivro")]
        public IActionResult AlterarLivro(LivroModel livroModel)
        {
            _livroRepository.AlterarLivro(livroModel);
            return Ok();
        }

        /// <summary>
        /// Método para inativar/desativar o livro.
        /// </summary>
        /// <param name="codigoLivro">Informa o código do livro. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("InativacaoLivro")]
        public IActionResult InativacaoLivro(decimal codigoLivro)
        {
            _livroRepository.InativacaoLivro(codigoLivro);
            return Ok();
        }

    }
}
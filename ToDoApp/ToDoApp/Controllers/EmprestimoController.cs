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
    public class EmprestimoController : MainController
    {
        IEmprestimoRepository _emprestimoRepository = null;
        IReservaRepository _reservaRepository = null;

        public EmprestimoController(IEmprestimoRepository emprestimoRepository, IReservaRepository reservaRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _reservaRepository = reservaRepository;
        }

        /// <summary>
        /// Método para recuperar o empréstimo pelo código.
        /// </summary>
        /// <param name="codigoEmprestimo">informar o código de empréstimo [decimal]</param>
        /// <returns>Objeto Empréstimo</returns>
        /// <response code="200">Objeto do tipo de empréstimo especifico.</response>
        [HttpGet("RecuperarEmprestimo")]
        public async Task<ActionResult> RecuperarEmprestimo(decimal codigoEmprestimo)
        {
            return Ok(await _emprestimoRepository.RecuperarEmprestimo(codigoEmprestimo));
        }

        /// <summary>
        /// Método para recuperar uma lista de empréstimos.
        /// </summary>
        /// <returns>Lista empréstimos</returns>
        /// <response code="200">Lista do tipo de empréstimos.</response>
        [HttpGet("RecuperarEmprestimos")]
        public async Task<ActionResult> RecuperarEmprestimos()
        {
            DataSourceResult<EmprestimoModel> rExecucao = new DataSourceResult<EmprestimoModel>();
            rExecucao.Data = _emprestimoRepository.RecuperarEmprestimos().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para retornar os empréstimos por devolução
        /// </summary>
        /// <param name="temDevolucao">Informa sem tem devolução ou não tem. [int]</param>
        /// <returns>Lista de Empréstimos</returns>
        /// <response code="200">Lista de empréstimos por devoluções.</response>   
        [HttpGet("RecuperarEmprestimosPorDevolucao")]
        public async Task<ActionResult> RecuperarEmprestimosPorDevolucao(int temDevolucao)
        {
            DataSourceResult<EmprestimoModel> rExecucao = new DataSourceResult<EmprestimoModel>();
            rExecucao.Data = _emprestimoRepository.RecuperarEmprestimosPorDevolucao(temDevolucao).Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para validar se o usuário já fez mais de dois emprestimo
        /// </summary>
        /// <param name="codigoUsuraio">Informa o código do usuário. [decimal]</param>
        /// <returns>A quantidade emprestada</returns>
        /// <response code="200">A quantidade emprestada pelo usuário.</response>      
        [HttpGet("ValidaUsuarioEmpresteMaximo")]
        public async Task<ActionResult> ValidaUsuarioEmpresteMaximo(decimal codigoUsuraio)
        {
            DataSourceResult<decimal> rExecucao = new DataSourceResult<decimal>();
            rExecucao.Total = _emprestimoRepository.ValidaUsuarioEmpresteMaximo(codigoUsuraio).Result;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para validra se o usuário extrapolou a quantidade de dias permitido
        /// </summary>
        /// <returns>Lista empréstimos</returns>
        /// <response code="200">Lista do tipo de empréstimos.</response>
        [HttpGet("ValidaUsuarioEmprestimoExtrapolado")]
        public async Task<ActionResult> ValidaUsuarioEmprestimoExtrapolado()
        {
            DataSourceResult<EmprestimoModel> rExecucao = new DataSourceResult<EmprestimoModel>();
            rExecucao.Data = _emprestimoRepository.ValidaUsuarioEmprestimoExtrapolado().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para incluir um empréstimo.
        /// </summary>
        /// <param name="emprestimoModel">Informa o objeto do tipo usuário. [EmprestimoModel]</param>
        /// <returns>Objeto Resposta</returns>
        /// <response code="200">O código incluído na base de dados.</response>
        [HttpPost("IncluirEmprestimo")]
        public async Task<ActionResult> IncluirEmprestimo(EmprestimoModel emprestimoModel)
        {
            return Ok(await _emprestimoRepository.IncluirEmprestimo(emprestimoModel));
        }

        /// <summary>
        /// Método para alterar o empréstimo.
        /// </summary>
        /// <param name="emprestimoModel">Informa o objeto do tipo empréstimo. [EmprestimoModel]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("AlterarEmprestimo")]
        public IActionResult AlterarEmprestimo(EmprestimoModel emprestimoModel)
        {
            _emprestimoRepository.AlterarEmprestimo(emprestimoModel);
            return Ok();
        }

        /// <summary>
        /// Método para devolver o livro emprestado.
        /// </summary>
        /// <param name="codigoEmprestimo">Informa o código do empréstimo. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("DevolvidoEmprestimo")]
        public IActionResult DevolvidoEmprestimo(decimal codigoEmprestimo)
        {
            _emprestimoRepository.DevolvidoEmprestimo(codigoEmprestimo);

            _reservaRepository.RemoveReserva(codigoEmprestimo);
            return Ok();
        }

    }
}
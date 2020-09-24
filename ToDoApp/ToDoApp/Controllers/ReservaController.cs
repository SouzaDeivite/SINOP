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
    public class ReservaController : MainController
    {
        IReservaRepository _reservaRepository = null;

        public ReservaController(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        /// <summary>
        /// Método para recuperar a reserva pelo código.
        /// </summary>
        /// <param name="codigoReserva">informar o código da reserva [decimal]</param>
        /// <returns>Objeto Reserva</returns>
        /// <response code="200">Objeto do tipo de reserva especifico.</response>
        [HttpGet("RecuperarReserva")]
        public async Task<ActionResult> RecuperarReserva(decimal codigoReserva)
        {
            return Ok(await _reservaRepository.RecuperarReserva(codigoReserva));
        }

        /// <summary>
        /// Método para recuperar uma lista de reservas.
        /// </summary>
        /// <returns>Lista reservas</returns>
        /// <response code="200">Lista do tipo de reservas.</response>
        [HttpGet("RecuperarReservas")]
        public async Task<ActionResult> RecuperarReservas()
        {
            DataSourceResult<ReservaModel> rExecucao = new DataSourceResult<ReservaModel>();
            rExecucao.Data = _reservaRepository.RecuperarReservas().Result;
            rExecucao.Total = rExecucao.Data.ToList().Count;
            return Ok(await Task.FromResult(rExecucao));
        }

        /// <summary>
        /// Método para incluir uma reserva.
        /// </summary>
        /// <param name="reservaModel">Informa o objeto do tipo reserva. [ReservaModel]</param>
        /// <returns>Objeto Resposta</returns>
        /// <response code="200">O código incluído na base de dados.</response>
        [HttpPost("IncluirReserva")]
        public async Task<ActionResult> IncluirReserva(ReservaModel reservaModel)
        {
            return Ok(await _reservaRepository.IncluirReserva(reservaModel));
        }

        /// <summary>
        /// Método para alterar a reserva.
        /// </summary>
        /// <param name="reservaModel">Informa o objeto do tipo reserva. [ReservaModel]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("AlterarReserva")]
        public IActionResult AlterarReserva(ReservaModel reservaModel)
        {
            _reservaRepository.AlterarReserva(reservaModel);
            return Ok();
        }

        /// <summary>
        /// Método para inativar/desativar a reserva.
        /// </summary>
        /// <param name="reservaModel">Informa o código da reserva. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>
        [HttpPut("InativacaoReserva")]
        public IActionResult InativacaoReserva(ReservaModel reservaModel)
        {
            _reservaRepository.InativacaoReserva(reservaModel);
            return Ok();
        }

        /// <summary>
        /// Método para incluir um livro na reserva.
        /// </summary>
        /// <param name="codigoEmprestimo">Informa o código do emprestimo. [decimal]</param>
        /// <returns></returns>
        /// <response code="200">Não existem retorno para este método.</response>/// 
        [HttpPut("IncluirLivroReserva")]
        public IActionResult IncluirLivroReserva(decimal codigoEmprestimo)
        {
            _reservaRepository.IncluirLivroReserva(codigoEmprestimo);
            return Ok();
        }        

    }
}
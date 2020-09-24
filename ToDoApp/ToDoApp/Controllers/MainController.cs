// importações das bibliotecas
using ToDoApp.Query;
using ToDoApp.Enumerators;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
// name space do projeto
namespace ToDoApp.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ActionResult CustomResponse(ResultadoExecucaoQuery rExecucao)
        {
            if (rExecucao.ResultadoExecucaoEnum == (int)ResultadoExecucaoEnum.Sucesso)
                return Ok(rExecucao);
            else
                return BadRequest(rExecucao);
        }

        protected string GetUserFromToken()
        {
            var identity = (HttpContext.User.Identity as ClaimsIdentity);

            if (identity != null)
                return identity.Name;
            else
                return null;
        }
    }
}
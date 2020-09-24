// importações das bibliotecas
using Dapper.Contrib.Extensions;
// name space do projeto
namespace ToDoApp.Model
{
    [Table("dbo.Reserva")]
    public class ReservaModel
    {
        [ExplicitKey]
        public decimal CodigoReserva { get; set; }
        public decimal CodigoEmprestimo { get; set; }
        public int AtivoReserva { get; set; }
    }
}
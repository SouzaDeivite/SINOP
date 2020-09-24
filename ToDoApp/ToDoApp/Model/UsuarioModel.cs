// importações das bibliotecas
using Dapper.Contrib.Extensions;
// name space do projeto
namespace ToDoApp.Model
{
    [Table("dbo.Usuario")]
    public class UsuarioModel
    {
        [ExplicitKey]
        public decimal CodigoUsuario { get; set; }
        public decimal CodigoInstituicaoEnsino { get; set; }
        public string NomeUsuario { get; set; }
        public string EnderecoUsuario { get; set; }
        public string CpfUsuario { get; set; }
        public string TelefoneUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public int AtivoUsuario { get; set; }
    }
}
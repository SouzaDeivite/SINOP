// importações das bibliotecas
using Dapper.Contrib.Extensions;
// name space do projeto
namespace ToDoApp.Model
{
    [Table("dbo.InstituicaoEnsino")]
    public class InstituicaoEnsinoModel
    {
        [ExplicitKey]
        public decimal CodigoInstituicaoEnsino { get; set; }
        public string NomeInstituicaoEnsino { get; set; }
        public string EnderecoInstituicaoEnsino { get; set; }
        public string CnpjInstituicaoEnsino { get; set; }
        public string TelefoneInstituicaoEnsino { get; set; }
        public int AtivoInstituicaoEnsino { get; set; }
    }
}
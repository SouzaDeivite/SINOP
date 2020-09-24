// importações das bibliotecas
using Dapper.Contrib.Extensions;
// name space do projeto
namespace ToDoApp.Model
{
    [Table("dbo.Livro")]
    public class LivroModel
    {
        [ExplicitKey]
        public decimal CodigoLivro { get; set; }
        public string TituloLivro { get; set; }
        public string AutorLivro { get; set; }
        public string SinopseLivro { get; set; }
        public string CapaLivro { get; set; }
        public int AtivoLivro { get; set; }
    }
}
// importações das bibliotecas
using System;
using Dapper.Contrib.Extensions;
// name space do projeto
namespace ToDoApp.Model
{
    [Table("dbo.Emprestimo")]
    public class EmprestimoModel
    {
        [ExplicitKey]
        public decimal CodigoEmprestimo { get; set; }
        public decimal CodigoUsuario { get; set; }
        public decimal CodigoLivro { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public int DevolvidoEmprestimo { get; set; }

        #region proprety ignorate
        [Computed]
        public LivroModel livro { get; set; }
        [Computed]
        public UsuarioModel usuario { get; set; }
        [Computed]
        public decimal QtdeDiasExtrapolados { get; set; }
        [Computed]
        public int Reservado { get; set; }
        #endregion

    }
}
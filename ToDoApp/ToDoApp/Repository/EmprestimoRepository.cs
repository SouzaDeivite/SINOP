// importações das bibliotecas
using Dapper;
using System;
using System.Text;
using System.Linq;
using ToDoApp.Base;
using ToDoApp.Model;
using ToDoApp.Query;
using ToDoApp.Interface;
using System.Collections;
using ToDoApp.Enumerators;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
// name space do projeto
namespace ToDoApp.Repository
{
    public class EmprestimoRepository : BaseRepository<EmprestimoModel>, IEmprestimoRepository
    {
        readonly IDbContext _context;

        public EmprestimoRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
            _context = context;
        }

        public Task<EmprestimoModel> RecuperarEmprestimo(decimal codigoEmprestimo)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                EmprestimoModel Retorno = new EmprestimoModel();
                conexao.Open();
                Retorno = conexao.Get<EmprestimoModel>(codigoEmprestimo);
                return Task.FromResult(Retorno);
            }
        }

        public Task<List<EmprestimoModel>> RecuperarEmprestimos()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<EmprestimoModel> lstRetorno = new List<EmprestimoModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT E.CODIGOEMPRESTIMO ");
                sql.AppendLine("   	        ,E.CODIGOUSUARIO");
                sql.AppendLine("   	        ,E.DATAEMPRESTIMO");
                sql.AppendLine("   	        ,E.DEVOLVIDOEMPRESTIMO");
                sql.AppendLine("   	        ,DATEDIFF(DAY, CAST(E.DATAEMPRESTIMO AS DATE), cast(dateadd(day, -30, getdate()) as date)) AS QTDEDIASEXTRAPOLADOS");
                sql.AppendLine("   	        ,(SELECT COUNT(1) FROM RESERVA R WHERE R.CODIGOEMPRESTIMO = E.CODIGOEMPRESTIMO AND R.ATIVORESERVA = 1) AS RESERVADO ");
                sql.AppendLine("   	        ,L.CODIGOLIVRO");
                sql.AppendLine("   	        ,L.TITULOLIVRO");
                sql.AppendLine("   	        ,U.CODIGOUSUARIO");
                sql.AppendLine("   	        ,U.NOMEUSUARIO");                
                sql.AppendLine("        FROM EMPRESTIMO E ");
                sql.AppendLine("   LEFT JOIN LIVRO L ");
                sql.AppendLine("          ON L.CODIGOLIVRO = E.CODIGOLIVRO ");
                sql.AppendLine("   LEFT JOIN USUARIO U ");
                sql.AppendLine("          ON U.CODIGOUSUARIO = E.CODIGOUSUARIO ");
                sql.AppendLine("    ORDER BY E.DATAEMPRESTIMO ");
                #endregion

                IEnumerable entities = conexao.Query<EmprestimoModel, LivroModel, UsuarioModel, EmprestimoModel>(sql.ToString(), (e, l, U) => { e.livro = l; e.usuario = U; return e; }, splitOn: "CODIGOLIVRO,CODIGOUSUARIO").AsEnumerable();
                if (((IList)entities).Count != 0)
                {
                    lstRetorno = (List<EmprestimoModel>)(IList)entities;
                }
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<EmprestimoModel>> RecuperarEmprestimosPorDevolucao(int devolucao)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<EmprestimoModel> lstRetorno = new List<EmprestimoModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT E.CODIGOEMPRESTIMO ");
                sql.AppendLine("   	        ,E.CODIGOUSUARIO");
                sql.AppendLine("   	        ,E.CODIGOLIVRO");
                sql.AppendLine("   	        ,E.DATAEMPRESTIMO");
                sql.AppendLine("   	        ,E.DEVOLVIDOEMPRESTIMO");
                sql.AppendLine("        FROM EMPRESTIMO E ");
                sql.AppendLine("       WHERE E.DEVOLVIDOEMPRESTIMO = " + devolucao);
                #endregion

                IEnumerable entities = conexao.Query<EmprestimoModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<EmprestimoModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<int> ValidaUsuarioEmpresteMaximo(decimal codigoUsuario)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("         SELECT COUNT(1) AS QTDE ");
                sql.AppendLine("           FROM EMPRESTIMO E ");
                sql.AppendLine("          WHERE E.CODIGOUSUARIO = " + codigoUsuario);
                sql.AppendLine("            AND E.DEVOLVIDOEMPRESTIMO = 0 ");
                #endregion
                
                int qtde = 0;
                IEnumerable entities = conexao.Query<decimal>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) {
                    foreach (object value in entities)
                    {
                        qtde = Convert.ToInt32(value);
                    }
                }

                return Task.FromResult(qtde);
            }
        }

        public Task<List<EmprestimoModel>> ValidaUsuarioEmprestimoExtrapolado()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<EmprestimoModel> lstRetorno = new List<EmprestimoModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT E.CODIGOEMPRESTIMO ");
                sql.AppendLine("   	        ,E.CODIGOUSUARIO");                
                sql.AppendLine("   	        ,E.DATAEMPRESTIMO");
                sql.AppendLine("   	        ,E.DEVOLVIDOEMPRESTIMO");
                sql.AppendLine("   	        ,DATEDIFF(DAY, CAST(E.DATAEMPRESTIMO AS DATE), cast(dateadd(day, 0, getdate()) as date)) AS QTDEDIASEXTRAPOLADOS");
                sql.AppendLine("   	        ,L.CODIGOLIVRO");
                sql.AppendLine("   	        ,L.TITULOLIVRO");
                sql.AppendLine("   	        ,U.CODIGOUSUARIO");
                sql.AppendLine("   	        ,U.NOMEUSUARIO");
                sql.AppendLine("        FROM EMPRESTIMO E ");
                sql.AppendLine("   LEFT JOIN LIVRO L ");
                sql.AppendLine("          ON L.CODIGOLIVRO = E.CODIGOLIVRO ");
                sql.AppendLine("   LEFT JOIN USUARIO U ");
                sql.AppendLine("          ON U.CODIGOUSUARIO = E.CODIGOUSUARIO ");
                sql.AppendLine("       WHERE CAST(E.DATAEMPRESTIMO AS DATE) <  CAST( DATEADD (DAY,-30,GETDATE()) AS DATE) ");
                sql.AppendLine("         AND E.DEVOLVIDOEMPRESTIMO = 0 ");
                #endregion

                IEnumerable entities = conexao.Query<EmprestimoModel, LivroModel, UsuarioModel, EmprestimoModel>(sql.ToString(), (e, l, U) => { e.livro = l; e.usuario = U; return e; }, splitOn: "CODIGOLIVRO,CODIGOUSUARIO").AsEnumerable();
                if (((IList)entities).Count != 0)
                {
                    lstRetorno = (List<EmprestimoModel>)(IList)entities;
                }
                return Task.FromResult(lstRetorno);
            }
        }
   
        public async Task<ResultadoExecucaoQuery<decimal>> IncluirEmprestimo(EmprestimoModel emprestimoModel)
        {
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                DateTime localDate = DateTime.Now;
                var id = NextKey<EmprestimoModel>(null);
                emprestimoModel.CodigoEmprestimo = (decimal)id;
                emprestimoModel.DataEmprestimo = localDate;
                await Insert(emprestimoModel);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> AlterarEmprestimo(EmprestimoModel emprestimoModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                await Update(emprestimoModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public Task<ResultadoExecucaoQuery> DevolvidoEmprestimo(decimal codigoEmprestimo)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();

            try
            {
                StringBuilder sql = new StringBuilder();

                #region
                sql.AppendLine(" UPDATE EMPRESTIMO ");
                sql.AppendLine("    SET DEVOLVIDOEMPRESTIMO = 1");
                sql.AppendLine("  WHERE CODIGOEMPRESTIMO = " + codigoEmprestimo);
                #endregion

                _context.Connection.Execute(sql.ToString());
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {

                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }

            return Task.FromResult(rExec);
        }

    }
}
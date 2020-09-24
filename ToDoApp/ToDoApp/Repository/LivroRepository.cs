// importações das bibliotecas
using Dapper;
using System;
using System.Linq;
using System.Text;
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
    public class LivroRepository : BaseRepository<LivroModel>, ILivroRepository
    {

        public LivroRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
        }

        public Task<LivroModel> RecuperarLivro(decimal codigoLivro)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                LivroModel Retorno = new LivroModel();
                conexao.Open();
                Retorno = conexao.Get<LivroModel>(codigoLivro);
                return Task.FromResult(Retorno);
            }
        }

        public Task<List<LivroModel>> RecuperarLivros()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                List<LivroModel> lstRetorno = new List<LivroModel>();
                conexao.Open();
                lstRetorno = conexao.GetAll<LivroModel>().ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<LivroModel>> RecuperarLivrosDisponiveisEmprestimo()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<LivroModel> lstRetorno = new List<LivroModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("         SELECT L.CODIGOLIVRO ");
                sql.AppendLine("               ,L.TITULOLIVRO ");
                sql.AppendLine("           FROM LIVRO L ");
                sql.AppendLine("          WHERE L.ATIVOLIVRO = 1 ");
                sql.AppendLine("        AND NOT EXISTS( SELECT NULL ");
                sql.AppendLine("                         FROM EMPRESTIMO E ");
                sql.AppendLine("                        WHERE E.DEVOLVIDOEMPRESTIMO = 0 ");
                sql.AppendLine("                          AND E.CODIGOLIVRO = L.CODIGOLIVRO) ");
                #endregion

                IEnumerable entities = conexao.Query<LivroModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<LivroModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<LivroModel>> RecuperarLivrosCombo()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<LivroModel> lstRetorno = new List<LivroModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT U.CODIGOLIVRO ");
                sql.AppendLine("   	        ,U.TITULOLIVRO");
                sql.AppendLine("        FROM LIVRO L ");
                sql.AppendLine("   LEFT JOIN EMPRESTIMO E ");
                sql.AppendLine("          ON E.CODIGOLIVRO = L.CODIGOLIVRO ");
                sql.AppendLine("       WHERE R.ATIVOUSUARIO = 1");
                sql.AppendLine("         AND I.ATIVOINSTITUICAOENSINO = 1");
                #endregion

                IEnumerable entities = conexao.Query<LivroModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<LivroModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public async Task<ResultadoExecucaoQuery<decimal>> IncluirLivro(LivroModel livroModel)
        {
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                var id = NextKey<LivroModel>(null);
                livroModel.CodigoLivro = (decimal)id;
                await Insert(livroModel);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> AlterarLivro(LivroModel livroModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                await Update(livroModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> InativacaoLivro(decimal codigoLivro)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                using (SqlConnection conexao = new SqlConnection(ConnectionString))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(" UPDATE LIVRO  ");
                    sql.AppendLine("    SET ATIVOLIVRO = 0 ");
                    sql.AppendLine("  WHERE CODIGOLIVRO = @CODIGOLIVRO ");
                    conexao.Execute(sql.ToString(), new { CODIGOLIVRO = codigoLivro });
                }
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

    }
}
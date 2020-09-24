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
    public class UsuarioRepository : BaseRepository<UsuarioModel>, IUsuarioRepository
    {

        public UsuarioRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
        }

        public Task<UsuarioModel> RecuperarUsuario(decimal codigoUsuario)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                UsuarioModel Retorno = new UsuarioModel();
                conexao.Open();
                Retorno = conexao.Get<UsuarioModel>(codigoUsuario);
                return Task.FromResult(Retorno);
            }
        }

        public Task<List<UsuarioModel>> RecuperarUsuarios()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                List<UsuarioModel> lstRetorno = new List<UsuarioModel>();
                conexao.Open();
                lstRetorno = conexao.GetAll<UsuarioModel>().ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<UsuarioModel>> RecuperarUsuariosDisponiveisEmprestimo()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<UsuarioModel> lstRetorno = new List<UsuarioModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("                 SELECT Y.CODIGOUSUARIO,  ");
                sql.AppendLine("                        Y.NOMEUSUARIO ");
                sql.AppendLine("                   FROM (SELECT U.CODIGOUSUARIO ");
                sql.AppendLine("                               ,U.NOMEUSUARIO ");
                sql.AppendLine("                               ,COALESCE(X.QTDE, 0) AS QTDE ");
                sql.AppendLine("                           FROM USUARIO U ");
                sql.AppendLine("                      LEFT JOIN (   SELECT CODIGOUSUARIO ");
                sql.AppendLine("                                          ,COUNT(1) AS QTDE ");
                sql.AppendLine("                                      FROM EMPRESTIMO ");
                sql.AppendLine("                                     WHERE DEVOLVIDOEMPRESTIMO = 0 ");
                sql.AppendLine("                                  GROUP BY CODIGOUSUARIO) X ");
                sql.AppendLine("                             ON X.CODIGOUSUARIO = U.CODIGOUSUARIO ");
                sql.AppendLine("                  WHERE U.ATIVOUSUARIO = 1 ");
                sql.AppendLine("             AND EXISTS(SELECT NULL ");
                sql.AppendLine("                            FROM INSTITUICAOENSINO I ");
                sql.AppendLine("                           WHERE I.ATIVOINSTITUICAOENSINO = 1 ");
                sql.AppendLine("                             AND I.CODIGOINSTITUICAOENSINO = U.CODIGOINSTITUICAOENSINO)) Y ");
                sql.AppendLine("                         WHERE Y.QTDE < 3 ");
                #endregion

                IEnumerable entities = conexao.Query<UsuarioModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<UsuarioModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }

         public async Task<ResultadoExecucaoQuery<decimal>> IncluirUsuario(UsuarioModel usuarioModel)
        {
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                var id = NextKey<UsuarioModel>(null);
                usuarioModel.CodigoUsuario = (decimal)id;
                await Insert(usuarioModel);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> AlterarUsuario(UsuarioModel usuarioModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                await Update(usuarioModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> InativacaoUsuario(decimal codigoUsuraio)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                using (SqlConnection conexao = new SqlConnection(ConnectionString))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(" UPDATE USUARIO  ");
                    sql.AppendLine("    SET ATIVOUSUARIO = 0 ");
                    sql.AppendLine("  WHERE CODIGOUSUARIO = @CODIGOUSUARIO ");
                    conexao.Execute(sql.ToString(), new { CODIGOUSUARIO = codigoUsuraio });
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
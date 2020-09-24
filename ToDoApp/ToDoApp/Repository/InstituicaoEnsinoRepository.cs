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
    public class InstituicaoEnsinoRepository : BaseRepository<InstituicaoEnsinoModel>, IInstituicaoEnsinoRepository
    {

        public InstituicaoEnsinoRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
        }

        public Task<InstituicaoEnsinoModel> RecuperarInstituicaoEnsino(decimal codigoInstituicaoEnsinoModel)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                InstituicaoEnsinoModel Retorno = new InstituicaoEnsinoModel();
                conexao.Open();
                Retorno = conexao.Get<InstituicaoEnsinoModel>(codigoInstituicaoEnsinoModel);
                return Task.FromResult(Retorno);
            }
        }

        public Task<List<InstituicaoEnsinoModel>> RecuperarInstituicoesEnsino()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                List<InstituicaoEnsinoModel> lstRetorno = new List<InstituicaoEnsinoModel>();
                conexao.Open();
                lstRetorno = conexao.GetAll<InstituicaoEnsinoModel>().ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<InstituicaoEnsinoModel>> RecuperarInstituicoesEnsinoAtivos()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<InstituicaoEnsinoModel> lstRetorno = new List<InstituicaoEnsinoModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT I.CODIGOINSTITUICAOENSINO ");
                sql.AppendLine("   	        ,I.NOMEINSTITUICAOENSINO");
                sql.AppendLine("   	        ,I.ENDERECOINSTITUICAOENSINO");
                sql.AppendLine("   	        ,I.CNPJINSTITUICAOENSINO");
                sql.AppendLine("   	        ,I.TELEFONEINSTITUICAOENSINO");
                sql.AppendLine("   	        ,I.ATIVOINSTITUICAOENSINO");
                sql.AppendLine("        FROM INSTITUICAOENSINO I ");
                sql.AppendLine("       WHERE I.ATIVOINSTITUICAOENSINO = 1");
                sql.AppendLine("    ORDER BY I.NOMEINSTITUICAOENSINO");
                #endregion

                IEnumerable entities = conexao.Query<InstituicaoEnsinoModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<InstituicaoEnsinoModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public async Task<ResultadoExecucaoQuery<decimal>> IncluirInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel)
        {
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                var id = NextKey<InstituicaoEnsinoModel>(null);
                instituicaoEnsinoModel.CodigoInstituicaoEnsino = (decimal)id;
                await Insert(instituicaoEnsinoModel);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> AlterarInstituicaoEnsino(InstituicaoEnsinoModel instituicaoEnsinoModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                await Update(instituicaoEnsinoModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> InativacaoInstituicaoEnsino(decimal codigoInstituicaoEnsino)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                using (SqlConnection conexao = new SqlConnection(ConnectionString))
                {
                    StringBuilder sql = new StringBuilder();

                    #region
                    sql.AppendLine(" UPDATE INSTITUICAOENSINO  ");
                    sql.AppendLine("    SET ATIVOINSTITUICAOENSINO = 0 ");
                    sql.AppendLine("  WHERE CODIGOINSTITUICAOENSINO = @CODIGOINSTITUICAOENSINO ");
                    #endregion

                    conexao.Execute(sql.ToString(), new { CODIGOINSTITUICAOENSINO = codigoInstituicaoEnsino });
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
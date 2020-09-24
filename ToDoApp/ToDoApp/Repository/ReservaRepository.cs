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
    public class ReservaRepository : BaseRepository<ReservaModel>, IReservaRepository
    {
        readonly IDbContext _context;
        public ReservaRepository(IConfiguration config, IDbContext context) : base(config, context)
        {
            _context = context;
        }

        public Task<ReservaModel> RecuperarReserva(decimal codigoReserva)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                ReservaModel Retorno = new ReservaModel();
                conexao.Open();
                Retorno = conexao.Get<ReservaModel>(codigoReserva);
                return Task.FromResult(Retorno);
            }
        }

        public Task<List<ReservaModel>> RecuperarReservas()
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                List<ReservaModel> lstRetorno = new List<ReservaModel>();
                conexao.Open();
                lstRetorno = conexao.GetAll<ReservaModel>().ToList();
                return Task.FromResult(lstRetorno);
            }
        }

        public Task<List<ReservaModel>> RecuperarReservaPorEmprestimo(decimal codigoEmprestimo)
        {
            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                List<ReservaModel> lstRetorno = new List<ReservaModel>();
                conexao.Open();

                #region [ sql ]
                sql.AppendLine("      SELECT R.CODIGORESERVA ");
                sql.AppendLine("   	        ,R.CODIGOEMPRESTIMO");
                sql.AppendLine("   	        ,R.ATIVORESERVA");
                sql.AppendLine("        FROM RESERVA R ");
                sql.AppendLine("       WHERE R.CODIGOEMPRESTIMO = " + codigoEmprestimo);
                sql.AppendLine("         AND R.ATIVORESERVA = 1");
                #endregion

                IEnumerable entities = conexao.Query<ReservaModel>(sql.ToString()).AsEnumerable();
                if (((IList)entities).Count != 0) lstRetorno = ((IEnumerable<ReservaModel>)(IList)entities).ToList();
                return Task.FromResult(lstRetorno);
            }
        }      

        public async Task<ResultadoExecucaoQuery<decimal>> IncluirReserva(ReservaModel reservaModel)
        {
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                var id = NextKey<ReservaModel>(null);
                reservaModel.CodigoReserva = (decimal)id;
                await Insert(reservaModel);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> AlterarReserva(ReservaModel reservaModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                await Update(reservaModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public async Task<ResultadoExecucaoQuery> InativacaoReserva(ReservaModel reservaModel)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();
            try
            {
                ReservaModel rModel = await GetById(reservaModel.CodigoReserva, null);
                rModel.AtivoReserva = 0;
                await Update(rModel);
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            }
            catch (Exception ex)
            {
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
                rExec.Mensagem = ex.Message;
            }
            return rExec;
        }

        public Task<ResultadoExecucaoQuery> RemoveReserva(decimal codigoEmprestimo)
        {
            ResultadoExecucaoQuery rExec = new ResultadoExecucaoQuery();

            try
            {
                StringBuilder sql = new StringBuilder();

                #region
                sql.AppendLine(" UPDATE RESERVA ");
                sql.AppendLine("    SET ATIVORESERVA = 0");
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

        public async Task<ResultadoExecucaoQuery> IncluirLivroReserva(decimal codigoEmprestimo)
        {
            ReservaModel r = new ReservaModel();
            ResultadoExecucaoQuery<decimal> rExec = new ResultadoExecucaoQuery<decimal>();
            rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Sucesso;
            try
            {
                var id = NextKey<ReservaModel>(null);
                r.CodigoReserva = (decimal)id;
                r.CodigoEmprestimo = codigoEmprestimo;
                r.AtivoReserva = 1;
                await Insert(r);
                rExec.Data = Convert.ToDecimal(id);
            }
            catch (Exception ex)
            {
                rExec.Excecao = ex;
                rExec.ResultadoExecucaoEnum = (int)ResultadoExecucaoEnum.Erro;
            }
            return rExec;
        }
       
    }
}
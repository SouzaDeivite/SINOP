// importações das bibliotecas
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Query
{
    public class ResultadoExecucaoQuery
    {
        private int _resultadoExecucaoEnum;

        public ResultadoExecucaoQuery()
        {
            this.ResultadoExecucaoEnum = (int)Enumerators.ResultadoExecucaoEnum.NaoEspecificado;
        }

        public ResultadoExecucaoQuery(int resultadoExecucaoEnum)
        {
            this.ResultadoExecucaoEnum = resultadoExecucaoEnum;
        }

        public ResultadoExecucaoQuery(int resultadoExecucaoEnum, Exception ex)
        {
            this.ResultadoExecucaoEnum = resultadoExecucaoEnum;
            this.Excecao = ex;
        }

        public int ResultadoExecucaoEnum
        {
            get
            {
                return _resultadoExecucaoEnum;
            }
            set
            {
                _resultadoExecucaoEnum = value;

                if (value == (int)Enumerators.ResultadoExecucaoEnum.Sucesso)
                    ResultadoExecucaoDescricao = "Sucesso";
                else if (value == (int)Enumerators.ResultadoExecucaoEnum.Erro)
                    ResultadoExecucaoDescricao = "Erro";
                else if (value == (int)Enumerators.ResultadoExecucaoEnum.NaoAutorizado)
                    ResultadoExecucaoDescricao = "Não Autorizado";
                else if (value == (int)Enumerators.ResultadoExecucaoEnum.NaoEspecificado)
                    ResultadoExecucaoDescricao = "Não Especificado";
                else if (value == (int)Enumerators.ResultadoExecucaoEnum.SessaoInvalida)
                    ResultadoExecucaoDescricao = "Sessão Inválida";
            }
        }

        public string ResultadoExecucaoDescricao { get; set; }
        public string Mensagem { get; set; }
        [JsonIgnore]
        public Exception Excecao
        {
            set
            {
                this.Mensagem = value.Message;
            }
        }
    }

    public class ResultadoExecucaoQuery<T> : ResultadoExecucaoQuery
    {
        public ResultadoExecucaoQuery()
        {
            this.ResultadoExecucaoEnum = (int)Enumerators.ResultadoExecucaoEnum.NaoEspecificado;
        }

        public ResultadoExecucaoQuery(T data)
        {
            this.Data = data;
            this.ResultadoExecucaoEnum = (int)Enumerators.ResultadoExecucaoEnum.Sucesso;
        }

        public T Data { get; set; }
    }

    public class ResultadoExecucaoListaQuery<T> : ResultadoExecucaoQuery
    {
        public ResultadoExecucaoListaQuery()
        {
            this.Data = new List<T>();
            this.ResultadoExecucaoEnum = (int)Enumerators.ResultadoExecucaoEnum.NaoEspecificado;
        }

        public ResultadoExecucaoListaQuery(List<T> itens)
        {
            this.Data = itens;
            this.Total = itens.Count;
            this.ResultadoExecucaoEnum = (int)Enumerators.ResultadoExecucaoEnum.Sucesso;
        }

        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}
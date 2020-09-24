// importações das bibliotecas
using System.Collections;
using System.Collections.Generic;
// name space do projeto
namespace ToDoApp.Class
{
    public class DataSourceResult
    {
        public IEnumerable Data { get; set; }

        public int Total { get; set; }
    }

    public class DataSourceResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }
    }
}
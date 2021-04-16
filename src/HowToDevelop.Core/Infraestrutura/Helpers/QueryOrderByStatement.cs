using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryOrderByStatement
    {
        public const string MensagemPosicaoRepetida = "Já existe um campo com a posição {0} informada!";
        private readonly List<QueryOrderBy> _queryOrders;
        public QueryOrderByStatement()
        {
            _queryOrders = new List<QueryOrderBy>();
        }

        public QueryOrderByStatement OrderBy(QueryOrderBy queryOrderBy)
        {
            if (_queryOrders.Any(o => o.Position == queryOrderBy.Position))
            {
                throw new QueryStatementException(string.Format(MensagemPosicaoRepetida, queryOrderBy.Position));
            }
            _queryOrders.Add(queryOrderBy);
            return this;
        }

        public override string ToString()
        {
            if (_queryOrders.Count <= 0)
            {
                return string.Empty;
            }
            var statement = new StringBuilder("Order by ");
            foreach (var order in _queryOrders.OrderBy(o => o.Position))
            {
                statement.Append($"{order}, ");
            }
            return statement.ToString().Remove(statement.Length -2);
        }
    }
}

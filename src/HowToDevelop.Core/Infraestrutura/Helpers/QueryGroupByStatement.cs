using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryGroupByStatement
    {
        public const string MensagemPosicaoRepetida = "Já existe um campo com a posição {0} informada!";
        private readonly List<QueryGroupBy> _queryGroups;
        public QueryGroupByStatement()
        {
            _queryGroups = new List<QueryGroupBy>();
        }

        public QueryGroupByStatement GroupBy(QueryGroupBy queryGroupBy)
        {
            if (_queryGroups.Any(o => o.Position == queryGroupBy.Position))
            {
                throw new QueryStatementException(string.Format(MensagemPosicaoRepetida, queryGroupBy.Position));
            }
            _queryGroups.Add(queryGroupBy);
            return this;
        }

        public override string ToString()
        {
            if (_queryGroups.Count <= 0)
            {
                return string.Empty;
            }

            var statement = new StringBuilder("Group by ");
            foreach (var group in _queryGroups.OrderBy(o => o.Position))
            {
                statement.Append($"{group}, ");
            }
            return statement.ToString().Remove(statement.Length -2);
        }

    }
}

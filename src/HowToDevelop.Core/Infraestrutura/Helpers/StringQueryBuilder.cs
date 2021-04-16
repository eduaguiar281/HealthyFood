using System.Text;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class StringQueryBuilder
    {
        public const string MensagemPaginationNaoEhValido = "Para adicionar paginação na query verifique se há a clausula ''ORDER BY'' adicionada e/ou ''OVER () as''";
        private readonly string _selectStatement;
        private readonly QueryOrderByStatement _orderBy;
        private readonly QueryGroupByStatement _groubBy;
        private QueryPaginationStatement _pagination;

        public StringQueryBuilder(in string selectStatement)
        {
            _selectStatement = selectStatement;
            MainCriteria = new AndAlsoCriteria();
            _orderBy = new QueryOrderByStatement();
            _groubBy = new QueryGroupByStatement();
        }

        public RootCriteria MainCriteria { get; set; }

        public StringQueryBuilder OrderBy(QueryOrderBy order)
        {
            _orderBy.OrderBy(order);
            return this;
        }
        public StringQueryBuilder GroupBy(QueryGroupBy group)
        {
            _groubBy.GroupBy(group);
            return this;
        }

        public void WithPagination(in int pageSize, in int pageNumber)
        {
            if (_orderBy.ToString() == string.Empty)
            {
                throw new QueryStatementException(MensagemPaginationNaoEhValido);
            }
            if (!_selectStatement.Contains("OVER () AS ", System.StringComparison.OrdinalIgnoreCase))
            {
                throw new QueryStatementException(MensagemPaginationNaoEhValido);
            }

            _pagination = new QueryPaginationStatement(pageSize, pageNumber);
        }

        public override string ToString()
        {
            var query = new StringBuilder();
            query.AppendLine(_selectStatement);

            string orderBy = _orderBy.ToString();
            string groupBy = _groubBy.ToString();
            string mainCriteria = MainCriteria.ToString();
            string pagination = _pagination != null ? _pagination.ToString() : string.Empty;

            if (!string.IsNullOrEmpty(mainCriteria))
            {
                query.AppendLine("WHERE " + mainCriteria);
            }
            if (!string.IsNullOrEmpty(groupBy))
            {
                query.AppendLine(groupBy);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query.AppendLine(orderBy);
            }
            if (!string.IsNullOrEmpty(pagination))
            {
                query.AppendLine(pagination);
            }
            return query.ToString();
        }
    }
}

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryPaginationStatement
    {
        public QueryPaginationStatement(in int pageSize, in int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public int PageSize { get; }
        public int PageNumber { get; }

        public override string ToString()
        {
            return $"OFFSET ({PageNumber}-1) * {PageSize} ROWS\r\nFETCH NEXT {PageSize} ROWS ONLY";
        }
    }
}

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryOrderBy
    {
        public QueryOrderBy(in string fieldName, in OrderByDirection direction, in int position)
        {
            FieldName = fieldName;
            Direction = direction;
            Position = position;
        }

        public int Position { get; }
        public string FieldName { get; }
        public OrderByDirection Direction { get; }

        private string ResolveDirection()
        {
            switch (Direction)
            {
                case OrderByDirection.Asc:
                    return "asc";
                case OrderByDirection.Desc:
                    return "desc";
                default:
                    return string.Empty;
            }
        }

        public override string ToString()
        {
            return $"{FieldName} {ResolveDirection()}";
        }
    }
}

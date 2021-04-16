namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryGroupBy
    {
        public QueryGroupBy(in string fieldName, in int position)
        {
            FieldName = fieldName;
            Position = position;
        }

        public int Position { get; }
        public string FieldName { get; }

        public override string ToString()
        {
            return $"{FieldName}";
        }

    }
}

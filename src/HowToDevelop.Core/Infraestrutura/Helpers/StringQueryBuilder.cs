namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class StringQueryBuilder
    {
        private readonly string _selectStatement;
        public StringQueryBuilder(in string selectStatement)
        {
            _selectStatement = selectStatement;
            MainCriteria = new AndAlsoCriteria();
        }

        public RootCriteria MainCriteria { get; set; }

        public override string ToString()
        {
            if (MainCriteria.ToString() == string.Empty)
            {
                return _selectStatement;
            }
            else
            {
                return $"{_selectStatement} WHERE {MainCriteria}";
            }
        }
    }
}

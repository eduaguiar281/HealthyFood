namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class SingleCriteria : Criteria
    {
        public SingleCriteria(in QueryCondition condition)
        {
            Condition = condition;
        }

        public QueryCondition Condition { get; }

        public override string ToString()
        {
            return Condition.ToString();
        }

    }
}

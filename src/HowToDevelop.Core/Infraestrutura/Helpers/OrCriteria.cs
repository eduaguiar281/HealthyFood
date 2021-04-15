namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class OrCriteria : SingleCriteria
    {
        public OrCriteria(in QueryCondition condition)
            : base(condition)
        {
        }
    }
}

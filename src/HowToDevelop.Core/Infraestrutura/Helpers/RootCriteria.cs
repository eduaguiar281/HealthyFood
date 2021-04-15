using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public abstract class RootCriteria : Criteria
    {
        protected List<Criteria> Childrens { get; set; }
        protected RootCriteria()
        {
            Childrens = new List<Criteria>();
        }

        public RootCriteria And(in QueryCondition conditon)
        {
            Childrens.Add(new AndCriteria(conditon));
            return this;
        }

        public RootCriteria AndAlso(in QueryCondition conditon)
        {
            var andAlso = new AndAlsoCriteria();
            Childrens.Add(andAlso);
            return andAlso.And(conditon);
        }

        public RootCriteria Or(in QueryCondition conditon)
        {
            Childrens.Add(new OrCriteria(conditon));
            return this;
        }

        public RootCriteria OrAlso(in QueryCondition conditon)
        {
            var orAlso = new OrAlsoCriteria();
            Childrens.Add(orAlso);
            return orAlso.Or(conditon);
        }

        private string ResolvePrintCriteria(Criteria criteria)
        {
            switch (criteria)
            {
                case SingleCriteria:
                    return $"{(criteria as SingleCriteria)}";
                case RootCriteria:
                    return $"{(criteria as RootCriteria)}";
                default:
                    return string.Empty;
            }
        }
        private string ResolvePrintCriteriaWithOperator(Criteria criteria)
        {
            switch (criteria)
            {
                case AndCriteria:
                    return $"AND {ResolvePrintCriteria(criteria)}";
                case OrCriteria:
                    return $"OR {ResolvePrintCriteria(criteria)}";
                case AndAlsoCriteria:
                    return $"AND ({ResolvePrintCriteria(criteria)})";
                case OrAlsoCriteria:
                    return $"OR ({ResolvePrintCriteria(criteria)})";
                default:
                    return string.Empty;
            }

        }

        private Dictionary<string, object> ResolveParameters(Criteria criteria)
        {
            switch (criteria)
            {
                case SingleCriteria:
                    {
                        Tuple<string, object> parameter = (criteria as SingleCriteria).Condition.GetParameter();
                        var result = new Dictionary<string, object>();
                        result.Add(parameter.Item1, parameter.Item2);
                        return result;
                    }
                case RootCriteria:
                    return (criteria as RootCriteria).GetParameters() ;
                default:
                    return new Dictionary<string, object>();
            }

        }

        public Dictionary<string, object> GetParameters()
        {
            var results = new Dictionary<string, object>();
            foreach (Criteria criteria in Childrens)
            {
                foreach(var item in ResolveParameters(criteria))
                {
                    results.Add(item.Key, item.Value);
                }
            }
            return results;
        }

        public override string ToString()
        {
            bool isFirst = true;
            var statement = new StringBuilder();
            foreach (Criteria criteria in Childrens)
            {
                if (isFirst)
                {
                    statement.AppendLine(ResolvePrintCriteria(criteria));
                    isFirst = false;
                }
                else
                {
                    statement.AppendLine(ResolvePrintCriteriaWithOperator(criteria));
                }
            }
            return statement.ToString();
        }
    }
}

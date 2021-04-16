using System;

namespace HowToDevelop.Core.Infraestrutura.Helpers
{
    public class QueryCondition
    {
        public QueryCondition(in string fieldName, in ComparisonOperator comparisonOperator,
            in object parameter, in string parameterName)
        {
            FieldName = fieldName;
            Operator = comparisonOperator;
            Parameter = parameter;
            ParameterName = parameterName;
        }
        public string FieldName { get; }
        public ComparisonOperator Operator { get; }
        public object Parameter { get; }
        public string ParameterName { get; }
        public override string ToString()
        {
            switch (Operator)
            {
                case ComparisonOperator.Equal:
                    return $"{FieldName} = {ParameterName}";
                case ComparisonOperator.NotEqual:
                    return $"{FieldName} <> {ParameterName}";
                case ComparisonOperator.Greater:
                    return $"{FieldName} > {ParameterName}";
                case ComparisonOperator.Less:
                    return $"{FieldName} < {ParameterName}";
                case ComparisonOperator.GreaterOrEqual:
                    return $"{FieldName} >= {ParameterName}";
                case ComparisonOperator.LessOrEqual:
                    return $"{FieldName} <= {ParameterName}";
                case ComparisonOperator.Like:
                    return $"{FieldName} like {ParameterName}";
                case ComparisonOperator.In:
                    return $"{FieldName} IN ({ParameterName})";
                default:
                    return string.Empty;
            }
        }

        public Tuple<string, object> GetParameter()
        {
            return new Tuple<string, object>(ParameterName, Parameter);
        }
    }
}

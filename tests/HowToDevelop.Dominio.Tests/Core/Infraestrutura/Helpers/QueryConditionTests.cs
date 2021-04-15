using HowToDevelop.Core.Infraestrutura.Helpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.Infraestrutura.Helpers
{
    public class QueryConditionTests
    {
        [Theory(DisplayName = "QueryCondition Converter Para String. Deve Ter Sucesso")]
        [Trait(nameof(QueryCondition), nameof(QueryCondition.ToString))]
        [InlineData("Codigo", ComparisonOperator.Equal, "@Codigo", "Codigo = @Codigo")]
        [InlineData("Codigo", ComparisonOperator.Greater, "@Codigo", "Codigo > @Codigo")]
        [InlineData("Codigo", ComparisonOperator.Less, "@Codigo", "Codigo < @Codigo")]
        [InlineData("Codigo", ComparisonOperator.GreaterOrEqual, "@Codigo", "Codigo >= @Codigo")]
        [InlineData("Codigo", ComparisonOperator.LessOrEqual, "@Codigo", "Codigo <= @Codigo")]
        [InlineData("Codigo", ComparisonOperator.NotEqual, "@Codigo", "Codigo <> @Codigo")]
        [InlineData("Nome", ComparisonOperator.Like, "@Nome", "Nome like @Nome")]
        [InlineData("Codigo", ComparisonOperator.In, "@Codigo", "Codigo IN (@Codigo)")]
        public void QueryCondition_ToString_DeveTerSucesso(string fieldName, ComparisonOperator comparsion, string parameterName, string expressionAssertion)
        {
            // Arrange 
            var queryCondition = new QueryCondition(fieldName, comparsion, 1, parameterName);

            // Act
            string resultAssertion = queryCondition.ToString();

            // Assert
            resultAssertion.ShouldBe(expressionAssertion);
        }

    }
}

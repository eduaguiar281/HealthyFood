using HowToDevelop.Core.Infraestrutura.Helpers;
using Shouldly;
using System;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.Infraestrutura.Helpers
{
    public class QueryGroupByStatementTests
    {
        [Fact(DisplayName = "QueryGroupByStatement GroupBy. Deve Ter Sucesso")]
        [Trait(nameof(QueryGroupByStatement), nameof(QueryGroupByStatement.GroupBy))]
        public void QueryGroupByStatement_GroupBy_DeveTerSucesso()
        {
            // Arrange
            string groupByStatementAssertion = "Group by Codigo, Nome, DataInclusao";
            var groupBy = new QueryGroupByStatement();
            groupBy.GroupBy(new QueryGroupBy("Codigo", 1))
                .GroupBy(new QueryGroupBy("Nome", 2))
                .GroupBy(new QueryGroupBy("DataInclusao", 3));

            // Act
            string result = groupBy.ToString();

            // Assert
            result.ShouldBe(groupByStatementAssertion);
        }

        [Fact(DisplayName = "GroupBy Sem Itens. Deve Ter Sucesso")]
        [Trait(nameof(QueryGroupByStatement), nameof(QueryGroupByStatement.GroupBy))]
        public void GroupBy_SemItens_DeveTerSucesso()
        {
            // Arrange
            var groupBy = new QueryGroupByStatement();

            // Act
            string result = groupBy.ToString();

            // Assert
            result.ShouldBe(string.Empty);
        }

        [Fact(DisplayName = "QueryGroupByStatement GroupBy, Posição repetida. Deve Falhar")]
        [Trait(nameof(QueryGroupByStatement), nameof(QueryGroupByStatement.GroupBy))]
        public void GroupBy_PosicaoRepetida_DeveFalhar()
        {
            // Arrange
            var groupBy = new QueryGroupByStatement();

            // Act
            var result = Assert.Throws<QueryStatementException>(() =>
            {
                groupBy.GroupBy(new QueryGroupBy("Codigo", 1))
                       .GroupBy(new QueryGroupBy("Nome", 1))
                       .GroupBy(new QueryGroupBy("DataInclusao", 3));
            });

            // Assert
            result.Message.ShouldBe(string.Format(QueryGroupByStatement.MensagemPosicaoRepetida, 1));
        }

    }
}

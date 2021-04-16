using HowToDevelop.Core.Infraestrutura.Helpers;
using Shouldly;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.Infraestrutura.Helpers
{
    public class QueryOrderByStatementTests
    {
        [Fact(DisplayName = "QueryOrderByStatement OrderBy. Deve Ter Sucesso")]
        [Trait(nameof(QueryOrderByStatement), nameof(QueryOrderByStatement.OrderBy))]
        public void QueryOrderByStatement_OrderBy_DeveTerSucesso()
        {
            // Arrange
            string orderByStatementAssertion = "Order by Codigo asc, Nome asc, DataInclusao desc";
            var orderBy = new QueryOrderByStatement();
            orderBy.OrderBy(new QueryOrderBy("Codigo", OrderByDirection.Asc, 1))
                .OrderBy(new QueryOrderBy("Nome", OrderByDirection.Asc, 2))
                .OrderBy(new QueryOrderBy("DataInclusao", OrderByDirection.Desc, 3));

            // Act
            string result = orderBy.ToString();

            // Assert
            result.ShouldBe(orderByStatementAssertion);
        }

        [Fact(DisplayName = "GroupBy Sem Itens. Deve Ter Sucesso")]
        [Trait(nameof(QueryOrderByStatement), nameof(QueryOrderByStatement.OrderBy))]
        public void OrderBy_SemItens_DeveTerSucesso()
        {
            // Arrange
            var orderBy = new QueryOrderByStatement();

            // Act
            string result = orderBy.ToString();

            // Assert
            result.ShouldBe(string.Empty);
        }


        [Fact(DisplayName = "QueryOrderByStatement OrderBy, posição repetida. Deve Falhar")]
        [Trait(nameof(QueryOrderByStatement), nameof(QueryOrderByStatement.OrderBy))]
        public void OrderBy_PosicaoRepetida_DeveFalhar()
        {
            // Arrange
            var orderBy = new QueryOrderByStatement();

            // Act
            var result = Assert.Throws<QueryStatementException>(() =>
            {
                orderBy.OrderBy(new QueryOrderBy("Codigo", OrderByDirection.Asc, 1))
                       .OrderBy(new QueryOrderBy("Nome", OrderByDirection.Asc, 1))
                       .OrderBy(new QueryOrderBy("DataInclusao", OrderByDirection.Desc, 3));
            });

            // Assert
            result.Message.ShouldBe(string.Format(QueryOrderByStatement.MensagemPosicaoRepetida, 1));
        }
    }
}

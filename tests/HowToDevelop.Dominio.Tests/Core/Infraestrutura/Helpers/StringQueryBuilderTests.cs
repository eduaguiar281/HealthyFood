using HowToDevelop.Core.Infraestrutura.Helpers;
using Shouldly;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.Infraestrutura.Helpers
{
    public class StringQueryBuilderTests
    {
        [Fact(DisplayName = "StringQueryBuilder adicionando critério AND Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteriaAnd_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente WHERE Codigo = @codigo\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando critério OR Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteriaOR_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente WHERE Codigo = @codigo\r\nOR Id = @id\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .Or(new QueryCondition("Id", ComparisonOperator.Equal, 10, "@id"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }
        
        [Fact(DisplayName = "StringQueryBuilder adicionando critério AND Also. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteriaAndAlso_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente WHERE Codigo = @codigo\r\nAND (Ativo = @ativo\r\nAND Excluido = @excluido\r\n)\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .AndAlso(new QueryCondition("Ativo", ComparisonOperator.Equal, false, "@ativo"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, false, "@excluido"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando critério AND Also Com Or. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteriaAndAlsoComOr_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente WHERE Codigo = @codigo\r\nAND (Ativo = @ativo\r\nOR Excluido = @excluido\r\n)\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .AndAlso(new QueryCondition("Ativo", ComparisonOperator.Equal, false, "@ativo"))
                .Or(new QueryCondition("Excluido", ComparisonOperator.Equal, false, "@excluido"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder sem critério. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void SemMainCriteria_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando critério OR Also. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteriaOrAlso_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente WHERE Codigo = @codigo\r\nOR (Id = @id\r\nAND Excluido = @excluido\r\n)\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, 1, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, true, "@excluido"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

    }
}

using HowToDevelop.Core.Infraestrutura.Helpers;
using Shouldly;
using System.Collections.Generic;
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
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\n\r\n";
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
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nOR Id = @id\r\n\r\n";
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
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nAND (Ativo = @ativo\r\nAND Excluido = @excluido\r\n)\r\n\r\n";
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
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nAND (Ativo = @ativo\r\nOR Excluido = @excluido\r\n)\r\n\r\n";
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
            string sqlAssertion = "SELECT * FROM Cliente\r\n";
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
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nOR (Id = @id\r\nAND Excluido = @excluido\r\n)\r\n\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, 1, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, true, "@excluido"));

            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder Obter Parametros. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.ToString))]
        public void MainCriteria_GetParameters_DeveTerSucesso()
        {
            // Arrange
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            object paramCodigo = 1;
            object paramNome = "%Eduard%";
            object paramId = 10;
            object paramExcluido = true;
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, paramCodigo, "@codigo"))
                .And(new QueryCondition("Nome", ComparisonOperator.Like, paramNome, "@nome"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, paramId, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, paramExcluido, "@excluido"));

            // Act
            Dictionary<string, object> result = query.MainCriteria.GetParameters();

            // Assert
            result.Count.ShouldBe(4);
            result.ContainsKey("@codigo").ShouldBeTrue();
            result.ContainsKey("@nome").ShouldBeTrue();
            result.ContainsKey("@id").ShouldBeTrue();
            result.ContainsKey("@excluido").ShouldBeTrue();
            result.GetValueOrDefault("@codigo").ShouldBe(paramCodigo);
            result.GetValueOrDefault("@nome").ShouldBe(paramNome);
            result.GetValueOrDefault("@id").ShouldBe(paramId);
            result.GetValueOrDefault("@excluido").ShouldBe(paramExcluido);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando Order by. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.OrderBy))]
        public void OrderBy_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nOR (Id = @id\r\nAND Excluido = @excluido\r\n)\r\n\r\nOrder by Nome desc, Codigo asc\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, 1, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, true, "@excluido"));
            query.OrderBy(new QueryOrderBy("Nome", OrderByDirection.Desc, 1))
                .OrderBy(new QueryOrderBy("Codigo", OrderByDirection.Asc, 2));
            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando Group by. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.GroupBy))]
        public void GroupBy_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT * FROM Cliente\r\nWHERE Codigo = @codigo\r\nOR (Id = @id\r\nAND Excluido = @excluido\r\n)\r\n\r\nGroup by Nome, Codigo\r\n";
            var query = new StringQueryBuilder("SELECT * FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, 1, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, true, "@excluido"));
            query.GroupBy(new QueryGroupBy("Nome", 1))
                .GroupBy(new QueryGroupBy("Codigo", 2));
            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando Paginacao. Deve Ter Sucesso")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.WithPagination))]
        public void WithPagination_ToString_DeveTerSucesso()
        {
            // Arrange
            string sqlAssertion = "SELECT *, COUNT(id) Over () As Total FROM Cliente\r\nWHERE Codigo = @codigo\r\nOR (Id = @id\r\nAND Excluido = @excluido\r\n)\r\n\r\nOrder by Id asc, Codigo asc\r\nOFFSET (10-1) * 50 ROWS\r\nFETCH NEXT 50 ROWS ONLY\r\n";
            var query = new StringQueryBuilder("SELECT *, COUNT(id) Over () As Total FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"))
                .OrAlso(new QueryCondition("Id", ComparisonOperator.Equal, 1, "@id"))
                .And(new QueryCondition("Excluido", ComparisonOperator.Equal, true, "@excluido"));
            query.OrderBy(new QueryOrderBy("Id", OrderByDirection.Asc, 1))
                .OrderBy(new QueryOrderBy("Codigo", OrderByDirection.Asc, 2))
                .WithPagination(50, 10);
            // Act
            string result = query.ToString();

            // Assert
            result.ShouldBe(sqlAssertion);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando Paginacao Sem Order By. Deve Falhar")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.WithPagination))]
        public void WithPagination_SemOrderBy_DeveFalhar()
        {
            // Arrange
            var query = new StringQueryBuilder("SELECT *, COUNT(id) Over () As Total FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"));

            // Act
            var ex = Assert.Throws<QueryStatementException>(() => query.WithPagination(50, 10));

            // Assert
            ex.Message.ShouldBe(StringQueryBuilder.MensagemPaginationNaoEhValido);
        }

        [Fact(DisplayName = "StringQueryBuilder adicionando Paginacao Sem Over () as. Deve Falhar")]
        [Trait(nameof(StringQueryBuilder), nameof(StringQueryBuilder.WithPagination))]
        public void WithPagination_SemOverAs_DeveFalhar()
        {
            // Arrange
            var query = new StringQueryBuilder("SELECT *, COUNT(id) FROM Cliente");
            query.MainCriteria.And(new QueryCondition("Codigo", ComparisonOperator.Equal, 1, "@codigo"));
            query.OrderBy(new QueryOrderBy("Id", OrderByDirection.Asc, 1));

            // Act
            var ex = Assert.Throws<QueryStatementException>(() => query.WithPagination(50, 10));

            // Assert
            ex.Message.ShouldBe(StringQueryBuilder.MensagemPaginationNaoEhValido);
        }
    }
}

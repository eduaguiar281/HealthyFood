using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class DescricaoTests
    {
        [Theory(DisplayName = "Valores válidos. Deve Ter Sucesso")]
        [Trait(nameof(Descricao), nameof(Descricao.Criar))]
        [InlineData("Coca-Cola")]
        [InlineData("Fanta Laranja")]
        [InlineData("Cheese Burger")]
        [InlineData("Pastel de Queijo")]
        [InlineData("Sanduíche Natural")]
        [InlineData("Cerveja Skoll")]
        [InlineData("Cerveja Antartica")]
        public void Descricao_ValorValido_DeveTerSucesso(string descricao)
        {
            // Arrange & Act
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);

            // Assert
            descricaoResult.IsSuccess.ShouldBeTrue();
            descricaoResult.Value.Valor.ShouldBe(descricao);
        }

        [Fact(DisplayName = "Número de caracteres inferior ao mínimo. Deve falhar")]
        [Trait(nameof(Descricao), nameof(Descricao.Criar))]
        public void Descricao_NumeroCaracteresInferiorAoMinimo_DeveFalhar()
        {
            //Arrange
            string descricao = "A";

            //Act
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);

            //Assert
            descricaoResult.IsFailure.ShouldBeTrue();
            descricaoResult.Error.ShouldContain(string.Format(DescricaoConstantes.DescricaoDeveTerNoMinimo,
                DescricaoConstantes.DescricaoTamanhoMinimoPadrao));
        }

        [Fact(DisplayName = "Número de caracteres superior ao máximo. Deve falhar")]
        [Trait(nameof(Descricao), nameof(Descricao.Criar))]
        public void Descricao_NumeroCaracteresSuperiorAoMaximo_DeveFalhar()
        {
            // Arrange
            string descricao = "A".PadRight(DescricaoConstantes.DescricaoTamanhoMaximoPadrao + 1, 'x');

            // Act
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);

            // Assert
            descricaoResult.IsFailure.ShouldBeTrue();
            descricaoResult.Error.ShouldContain(string.Format(DescricaoConstantes.DescricaoDeveTerNoMaximo,
                DescricaoConstantes.DescricaoTamanhoMaximoPadrao));
        }

        [Theory(DisplayName = "Descricao nulo ou vazio. Deve falhar")]
        [Trait(nameof(Descricao), nameof(Descricao.Criar))]
        [InlineData(null)]
        [InlineData("")]
        public void Descricao_ValorNuloOuVazio_DeveFalhar(string descricao)
        {
            // Arrange & Act
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);

            // Assert
            descricaoResult.IsFailure.ShouldBeTrue();
            descricaoResult.Error.ShouldContain(DescricaoConstantes.DescricaoEhObrigatorio);
        }
    }
}

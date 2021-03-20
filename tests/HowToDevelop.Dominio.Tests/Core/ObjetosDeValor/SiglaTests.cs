using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class SiglaTests
    {
        [Theory(DisplayName = "Valores válidos. Deve Ter Sucesso")]
        [Trait(nameof(Sigla), nameof(Sigla.Criar))]
        [InlineData("AXT-01d")]
        [InlineData("VIP-01")]
        [InlineData("VAR-02")]
        [InlineData("INT-01")]
        [InlineData("EXT-01a")]
        [InlineData("")]
        [InlineData(null)]
        public void Sigla_ValorValido_DeveTerSucesso(string sigla)
        {
            // Arrange & Act
            Result<Sigla> siglaResult = Sigla.Criar(sigla);

            // Assert
            siglaResult.IsSuccess.ShouldBeTrue();
            siglaResult.Value.Valor.ShouldBe(sigla);
        }

        [Fact(DisplayName = "Número de caracteres superior ao máximo. Deve falhar")]
        [Trait(nameof(Sigla), nameof(Sigla.Criar))]
        public void Sigla_NumeroCaracteresSuperiorAoMaximo_DeveFalhar()
        {
            // Arrange
            string sigla = "A".PadRight(SiglaConstantes.SiglaTamanhoMaximoPadrao + 1, 'x');

            // Act
            Result<Sigla> siglaResult = Sigla.Criar(sigla);

            // Assert
            siglaResult.IsFailure.ShouldBeTrue();
            siglaResult.Error.ShouldContain(string.Format(SiglaConstantes.SiglaDeveTerNoMaximo,
                SiglaConstantes.SiglaTamanhoMaximoPadrao));
        }

        [Fact(DisplayName = "Conversao Implicita. Deve ter sucesso")]
        [Trait(nameof(Sigla), "Conversao")]
        public void Sigla_ConversaoImplicita_DeveTerSucesso()
        {
            // Arrange
            string sigla = "VIP-01";
            Sigla siglaObjeto = Sigla.Criar(sigla).Value;

            // Act
            string siglaConversao = siglaObjeto;

            // Assert
            siglaConversao.ShouldBe(sigla);
        }
    }
}

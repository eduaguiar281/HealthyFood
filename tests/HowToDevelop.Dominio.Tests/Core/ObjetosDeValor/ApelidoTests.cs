using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class ApelidoTests
    {
        [Theory(DisplayName = "Valores válidos. Deve Ter Sucesso")]
        [Trait(nameof(Apelido), nameof(Apelido.Criar))]
        [InlineData("Mariazinha")]
        [InlineData("Jão")]
        [InlineData("Luizão")]
        [InlineData("Edu")]
        [InlineData("Nanda")]
        [InlineData("Déia")]
        [InlineData("Rico")]
        [InlineData("")]
        [InlineData(null)]
        public void Apelido_ValorValido_DeveTerSucesso(string apelido)
        {
            // Arrange & Act
            Result<Apelido> apelidoResult = Apelido.Criar(apelido);

            // Assert
            apelidoResult.IsSuccess.ShouldBeTrue();
            apelidoResult.Value.Valor.ShouldBe(apelido);
        }

        [Fact(DisplayName = "Número de caracteres superior ao máximo. Deve falhar")]
        [Trait(nameof(Apelido), nameof(Apelido.Criar))]
        public void Apelido_NumeroCaracteresSuperiorAoMaximo_DeveFalhar()
        {
            // Arrange
            string apelido = "A".PadRight(ApelidoConstantes.ApelidoTamanhoMaximoPadrao + 1, 'x');

            // Act
            Result<Apelido> apelidoResult = Apelido.Criar(apelido);

            // Assert
            apelidoResult.IsFailure.ShouldBeTrue();
            apelidoResult.Error.ShouldContain(string.Format(ApelidoConstantes.ApelidoDeveTerNoMaximo,
                ApelidoConstantes.ApelidoTamanhoMaximoPadrao));
        }
    }
}

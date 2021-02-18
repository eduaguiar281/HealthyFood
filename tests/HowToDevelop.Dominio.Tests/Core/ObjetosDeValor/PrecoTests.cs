using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using System;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class PrecoTests
    {
        [Theory(DisplayName = "Criar Valores Validos Deve Ter Sucesso")]
        [Trait(nameof(Preco), "Criar")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Criar_ValoresValido_DeveTerSucesso(decimal valor)
        {
            //Arrange & Act
            Preco preco = new Preco(valor);

            //Assert
            preco.Valor.ShouldBe(valor);
        }

        [Theory(DisplayName = "Criar Valores Inválidos Deve Falhar")]
        [Trait(nameof(Preco), "Criar")]
        [InlineData(-1.95)]
        [InlineData(-1.99)]
        [InlineData(-550.32)]
        [InlineData(-18.99)]
        [InlineData(-3)]
        public void Criar_ValoresInvalido_DeveFalhar(decimal valor)
        {
            //Arrange 
            Action action = () => { new Preco(valor); };

            //Act
            var ex  = Assert.Throws<ArgumentException>(action);

            //Assert
            ex.Message.ShouldContain(PrecoConstantes.PrecoNaoPodeSerInferiorZero);
        }

        [Theory(DisplayName = "Conversao Implícita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Preco), "Conversao")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Preco_ConversaoImplicita_DeveTerSucesso(decimal valor)
        {
            //Arrange
            Preco preco = new Preco(valor);

            //Act
            decimal convertido = preco;

            //Assert
            convertido.ShouldBe(valor);
        }

        [Theory(DisplayName = "Conversao Explicita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Preco), "Conversao")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Preco_ConversaoExlicita_DeveTerSucesso(decimal valor)
        {
            //Arrange & Act
            Preco preco = (Preco)valor;

            //Assert
            preco.Valor.ShouldBe(valor);
        }
    }
}

using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class TotalTests
    {
        [Theory(DisplayName = "Criar Valores Validos Deve Ter Sucesso")]
        [Trait(nameof(Total), "Criar")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Criar_ValoresValido_DeveTerSucesso(decimal valor)
        {
            //Arrange & Act
            Total total = new Total(valor);

            //Assert
            total.Valor.ShouldBe(valor);
        }

        [Theory(DisplayName = "Criar Valores Inválidos Deve Falhar")]
        [Trait(nameof(Total), "Criar")]
        [InlineData(-1.95)]
        [InlineData(-1.99)]
        [InlineData(-550.32)]
        [InlineData(-18.99)]
        [InlineData(-3)]
        public void Criar_ValoresInvalido_DeveFalhar(decimal valor)
        {
            //Arrange 
            void action() { new Total(valor); }

            //Act
            var ex = Assert.Throws<ArgumentException>(action);

            //Assert
            ex.Message.ShouldContain(TotalConstantes.TotalNaoPodeSerInferiorZero);
        }

        [Theory(DisplayName = "Conversao Implícita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Total), "Conversao")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Preco_ConversaoImplicita_DeveTerSucesso(decimal valor)
        {
            //Arrange
            Total total = new Total(valor);

            //Act
            decimal convertido = total;

            //Assert
            convertido.ShouldBe(valor);
        }

        [Theory(DisplayName = "Conversao Explicita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Total), "Conversao")]
        [InlineData(1.95)]
        [InlineData(1.99)]
        [InlineData(550.32)]
        [InlineData(18.99)]
        [InlineData(3)]
        [InlineData(0)]
        public void Preco_ConversaoExlicita_DeveTerSucesso(decimal valor)
        {
            //Arrange & Act
            Total total = (Total)valor;

            //Assert
            total.Valor.ShouldBe(valor);
        }

    }
}

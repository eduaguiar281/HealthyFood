using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class QuantidadeTests
    {
        [Theory(DisplayName = "Criar Valores Validos Deve Ter Sucesso")]
        [Trait(nameof(Quantidade), "Criar")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(550)]
        [InlineData(18)]
        [InlineData(3)]
        public void Criar_ValoresValido_DeveTerSucesso(int valor)
        {
            //Arrange & Act
            Quantidade quantidade = new Quantidade(valor);

            //Assert
            quantidade.Valor.ShouldBe(valor);
        }

        [Theory(DisplayName = "Criar Valores Inválidos Deve Falhar")]
        [Trait(nameof(Quantidade), "Criar")]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-550)]
        [InlineData(-18)]
        [InlineData(-3)]
        [InlineData(0)]
        public void Criar_ValoresInvalido_DeveFalhar(int valor)
        {
            //Arrange 
            Action action = () => { new Quantidade(valor); };

            //Act
            var ex = Assert.Throws<ArgumentException>(action);

            //Assert
            ex.Message.ShouldContain(QuantidadeConstantes.QuantidadeNaoPodeSerInferiorOuIgualZero);
        }

        [Theory(DisplayName = "Conversao Implícita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Quantidade), "Conversao")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(550)]
        [InlineData(18)]
        [InlineData(3)]
        public void Quantidade_ConversaoImplicita_DeveTerSucesso(int valor)
        {
            //Arrange
            Quantidade quantidade = new Quantidade(valor);

            //Act
            int convertido = quantidade;

            //Assert
            convertido.ShouldBe(valor);
        }

        [Theory(DisplayName = "Conversao Explicita Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Quantidade), "Conversao")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(550)]
        [InlineData(18)]
        [InlineData(3)]
        public void Quantidade_ConversaoExlicita_DeveTerSucesso(int valor)
        {
            //Arrange & Act
            Quantidade quantidade = (Quantidade)valor;

            //Assert
            quantidade.Valor.ShouldBe(valor);
        }


    }
}

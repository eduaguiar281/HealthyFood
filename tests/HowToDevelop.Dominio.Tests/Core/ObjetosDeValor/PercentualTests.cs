using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Core.ObjetosDeValor
{
    public class PercentualTests
    {
        [Theory(DisplayName = "Valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Percentual), nameof(Percentual.Criar))]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(8.5)]
        [InlineData(15)]
        [InlineData(25.10)]
        [InlineData(28.25)]
        [InlineData(100)]
        public void Percentual_ValoresValido_DeveTerSucesso(decimal valor)
        {
            //Arrange 
            string percentualNominal = $"{valor:N1} %";
            decimal percentualValor = valor/100;

            //Act
            Result<Percentual> percentualResult = Percentual.Criar(valor);


            //Assert
            percentualResult.IsSuccess.ShouldBeTrue();
            percentualResult.Value.ValorNominal.ShouldBe(percentualNominal);
            percentualResult.Value.Valor.ShouldBe(percentualValor);
        }

        [Fact(DisplayName = "Valor menor que zero Deve Falhar")]
        [Trait(nameof(Percentual), nameof(Percentual.Criar))]
        public void Percentual_ValorMenorZero_DeveFalhar()
        {
            //Arrange 
            decimal valor = -1;

            //Act
            Result<Percentual> percentualResult = Percentual.Criar(valor);


            //Assert
            percentualResult.IsFailure.ShouldBeTrue();
            percentualResult.Error.ShouldContain(PercentualConstantes.PercentualNaoDeveSerMenorQueZero);
        }

        [Fact(DisplayName = "Valor maior que cem Deve Falhar")]
        [Trait(nameof(Percentual), nameof(Percentual.Criar))]
        public void Percentual_ValorMaiorQueCem_DeveFalhar()
        {
            //Arrange 
            decimal valor = 110.2m;

            //Act
            Result<Percentual> percentualResult = Percentual.Criar(valor);


            //Assert
            percentualResult.IsFailure.ShouldBeTrue();
            percentualResult.Error.ShouldContain(PercentualConstantes.PercentualNaoDeveSerMaiorQueCem);
        }

        [Fact(DisplayName = "Conversão Implicita Deve Ter Sucesso")]
        [Trait(nameof(Percentual), "Conversão Explicita")]
        public void Percentual_ConversaoImplicita_DeveTerSucesso()
        {
            //Arrange 
            Result<Percentual> percentualResult = Percentual.Criar(12.25m);

            //Act
            decimal valor = percentualResult.Value;

            //Assert
            percentualResult.IsSuccess.ShouldBeTrue();
            percentualResult.Value.Valor.ShouldBe(valor);
        }

    }
}

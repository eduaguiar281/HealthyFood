using Xunit;
using Shouldly;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;
using System;
using HowToDevelop.Core.ObjetosDeValor;

namespace HowToDevelop.Dominio.Tests.Dominio.Pedidos.ObjetosDeValor
{
    public class DescontoTests
    {
        [Theory(DisplayName = "Valor Válido Deve Ter Sucesso")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorValor))]
        [InlineData(10, 200, 0.05)]
        [InlineData(10, 100, 0.1)]
        [InlineData(13, 48.50, 0.27)]
        public void DescontoPorValor_DescontoValido_DeveTerSucesso(decimal valor, decimal baseCalculo, decimal percentualEsperado)
        {
            //Arrange & Act
            var desconto = Desconto.CriarPorValor(valor, baseCalculo);

            //Assert
            desconto.IsSuccess.ShouldBeTrue();
            desconto.Value.Valor.ShouldBe(valor);
            desconto.Value.BaseCalculo.ShouldBe(baseCalculo);
            Math.Round(desconto.Value.Percentual, 2).ShouldBe(percentualEsperado);
            desconto.Value.TipoDescontoPedido.ShouldBe(TipoDesconto.Valor);
        }

        [Fact(DisplayName = "Valor Negativo Deve Falhar")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorValor))]
        public void DescontoPorValor_ValorNegativo_DeveFalhar()
        {
            //Arrange 
            decimal valor = -10m;
            decimal baseCalculo = 200.99m;

            //Act
            var desconto = Desconto.CriarPorValor(valor, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoConstantes.ValorDescontoNaoPodeSerMenorQueZero);
        }

        [Fact(DisplayName = "Valor Base Calculo igual a zero Deve Falhar")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorValor))]
        public void DescontoPorValor_BaseCalculoZero_DeveFalhar()
        {
            //Arrange 
            decimal valor = 10m;
            decimal baseCalculo = 0m;

            //Act
            var desconto = Desconto.CriarPorValor(valor, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoConstantes.BaseDeCaluculoNaoPodeSerIgualZero);
        }

        [Theory(DisplayName = "Percentuais Validos Deve Ter Sucesso")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorPercentual))]
        [InlineData(100, 10, 10)]
        [InlineData(233.95, 15, 35.09)]
        [InlineData(25.15, 5, 1.26)]
        [InlineData(9, 3, 0.27)]
        public void DescontoPercentual_ValoresValidos_DeveTerSucesso(decimal baseCalculo, decimal percentual, decimal resultado)
        {
            //Arrange & Act
            var desconto = Desconto.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsSuccess.ShouldBeTrue();
            Math.Round(desconto.Value.Valor, 2).ShouldBe(resultado);
            desconto.Value.BaseCalculo.ShouldBe(baseCalculo);
            desconto.Value.Percentual.Valor.ShouldBe(percentual/100);
        }

        [Fact(DisplayName = "Percentual Zero Deve Falhar")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorPercentual))]
        public void DescontoPercentual_PercentualZero_DeveFalhar()
        {
            //Arrange 
            decimal percentual = 0;
            decimal baseCalculo = 100.99m;
            //Act
            var desconto = Desconto.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoConstantes.PercentualDeveSerMaiorQueZero);
        }

        [Fact(DisplayName = "Percentual Invalido Deve Falhar")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorPercentual))]
        public void DescontoPercentual_PercentualInvalid_DeveFalhar()
        {
            //Arrange 
            decimal percentual = 110;
            decimal baseCalculo = 100.99m;
            //Act
            var desconto = Desconto.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(PercentualConstantes.PercentualNaoDeveSerMaiorQueCem);
        }

        [Fact(DisplayName = "Base de Calculo zero Deve Falhar")]
        [Trait(nameof(Desconto), nameof(Desconto.CriarPorPercentual))]
        public void DescontoPercentual_BaseCalculoZero_DeveFalhar()
        {
            //Arrange 
            decimal percentual = 10;
            decimal baseCalculo = 0;
            //Act
            var desconto = Desconto.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoConstantes.BaseDeCaluculoNaoPodeSerIgualZero);
        }

        [Theory(DisplayName = "Conversao valores válidos Deve Ter Sucesso")]
        [Trait(nameof(Desconto), "Conversão")]
        [InlineData(10.99, 100)]
        [InlineData(4.492, 300)]
        [InlineData(3.45, 200.59)]
        [InlineData(13.57, 550.25)]
        public void Conversao_ValoresValidos_DeveTerSucesso(decimal valor, decimal baseCalculo)
        {
            //Arrange 
            var desconto = Desconto.CriarPorValor(valor, baseCalculo).Value;

            //Act
            decimal valorConvertido = desconto;

            //Assert
            valorConvertido.ShouldBe(desconto.Valor);
        }

        [Theory(DisplayName = "Conversao percentuais válidos Deve Ter Sucesso")]
        [Trait(nameof(Desconto), "Conversão")]
        [InlineData(5, 100)]
        [InlineData(15, 300)]
        [InlineData(20, 200.59)]
        [InlineData(23, 550.25)]
        public void Conversao_percentuais_DeveTerSucesso(decimal percentual, decimal baseCalculo)
        {
            //Arrange 
            var desconto = Desconto.CriarPorValor(percentual, baseCalculo).Value;

            //Act
            decimal valorConvertido = desconto;

            //Assert
            valorConvertido.ShouldBe(desconto.Valor);
        }
    }
}

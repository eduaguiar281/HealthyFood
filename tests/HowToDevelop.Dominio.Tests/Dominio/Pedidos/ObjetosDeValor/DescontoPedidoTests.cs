using Xunit;
using Shouldly;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;

namespace HowToDevelop.Dominio.Tests.Dominio.Pedidos.ObjetosDeValor
{
    public class DescontoPedidoTests
    {
        [Fact(DisplayName = "Valor Válido Deve Ter Sucesso")]
        [Trait(nameof(DescontoPedido), nameof(DescontoPedido.CriarPorValor))]
        public void DescontoPorValor_DescontoValido_DeveTerSucesso()
        {
            //Arrange 
            decimal valor = 10m;
            decimal baseCalculo = 200.99m;

            //Act
            var desconto = DescontoPedido.CriarPorValor(valor, baseCalculo);


            //Assert
            desconto.IsSuccess.ShouldBeTrue();
            desconto.Value.Valor.ShouldBe(valor);
            desconto.Value.BaseCalculo.ShouldBe(baseCalculo);
            desconto.Value.TipoDescontoPedido.ShouldBe(TipoDescontoPedido.Valor);
        }

        [Fact(DisplayName = "Valor Negativo Deve Falhar")]
        [Trait(nameof(DescontoPedido), nameof(DescontoPedido.CriarPorValor))]
        public void DescontoPorValor_ValorNegativo_DeveFalhar()
        {
            //Arrange 
            decimal valor = -10m;
            decimal baseCalculo = 200.99m;

            //Act
            var desconto = DescontoPedido.CriarPorValor(valor, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoPedidoConstantes.ValorDescontoNaoPodeSerMenorQueZero);
        }

        [Theory(DisplayName = "Percentuais Validos Deve Ter Sucesso")]
        [Trait(nameof(DescontoPedido), nameof(DescontoPedido.CriarPorPercentual))]
        [InlineData(100, 10, 10)]
        [InlineData(233.95, 15, 35.0925)]
        [InlineData(25.15, 5, 1.2575)]
        [InlineData(9, 3, 0.27)]
        public void DescontoPercentual_ValoresValidos_DeveTerSucesso(decimal baseCalculo, decimal percentual, decimal resultado)
        {
            //Arrange & Act
            var desconto = DescontoPedido.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsSuccess.ShouldBeTrue();
            desconto.Value.Valor.ShouldBe(resultado);
            desconto.Value.BaseCalculo.ShouldBe(baseCalculo);
            desconto.Value.Percentual.ShouldBe(percentual);
        }

        [Fact(DisplayName = "Percentual Zero Deve Falhar")]
        [Trait(nameof(DescontoPedido), nameof(DescontoPedido.CriarPorPercentual))]
        public void DescontoPercentual_PercentualZero_DeveFalhar()
        {
            //Arrange 
            decimal percentual = 0;
            decimal baseCalculo = 100.99m;
            //Act
            var desconto = DescontoPedido.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoPedidoConstantes.PercentualDeveSerMaiorQueZero);
        }

        [Fact(DisplayName = "Percentual acima do valor máximo Deve Falhar")]
        [Trait(nameof(DescontoPedido), nameof(DescontoPedido.CriarPorPercentual))]
        public void DescontoPercentual_AcimaDoPermitido_DeveFalhar()
        {
            //Arrange 
            decimal percentual = DescontoPedidoConstantes.PercentualMaximo + 1;
            decimal baseCalculo = 100.99m;
            //Act
            var desconto = DescontoPedido.CriarPorPercentual(percentual, baseCalculo);

            //Assert
            desconto.IsFailure.ShouldBeTrue();
            desconto.Error.ShouldContain(DescontoPedidoConstantes.PercentualDescontoNaoDeveUltrapassarPercentualMaximo);
        }
    }
}

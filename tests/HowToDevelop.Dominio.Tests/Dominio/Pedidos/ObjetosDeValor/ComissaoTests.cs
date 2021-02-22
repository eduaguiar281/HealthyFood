using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;
using Shouldly;
using System;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Pedidos.ObjetosDeValor
{
    public class ComissaoTests
    {

        [Theory(DisplayName = "Criar Comissão Valores Válidos Deve Ter Sucesso")]
        [Trait(nameof(Comissao), nameof(Comissao.Criar))]
        [InlineData(100, 10, 5, 15)]
        [InlineData(100, 10, 0, 10)]
        [InlineData(255, 12.5, 3, 34.88)]
        [InlineData(45.8, 5, 8, 10.29)]
        [InlineData(55.20, 0, 5, 5)]
        public void Criar_ValoresValidos_DeveTerSucesso(decimal baseCalculo, decimal percentual, decimal gorjeta, decimal totalEsperado)
        {
            //Arrange & Act
            var result = Comissao.Criar(baseCalculo, percentual, gorjeta);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.BaseCalculo.ShouldBe(baseCalculo);
            result.Value.Percentual.Valor.ShouldBe(percentual/100);
            result.Value.Gorjeta.ShouldBe(gorjeta);
            Math.Round(result.Value.Total, 2).ShouldBe(totalEsperado);
        }


        [Fact(DisplayName = "Criar Percentual inválido Deve Falhar")]
        [Trait(nameof(Comissao), nameof(Comissao.Criar))]
        public void Criar_PercentualInvalido_DeveFalhar()
        {
            //Arrange & Act
            var result = Comissao.Criar(100, 110, 0);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PercentualConstantes.PercentualNaoDeveSerMaiorQueCem);
        }

        [Fact(DisplayName = "Criar Base de Calculo Zero Deve Falhar")]
        [Trait(nameof(Comissao), nameof(Comissao.Criar))]
        public void Criar_BaseCalculoZero_DeveFalhar()
        {
            //Arrange & Act
            var result = Comissao.Criar(0, 25, 10);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ComissaoConstantes.BaseCalculoNaoPodeSerIgualZero);
        }
    }
}

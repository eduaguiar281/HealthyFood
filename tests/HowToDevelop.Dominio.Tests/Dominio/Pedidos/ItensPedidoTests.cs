using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Pedidos
{
    public class ItensPedidoTests
    {
        public ItensPedidoTests()
        {   }

        [Fact(DisplayName = "Válido Deve Ter Sucesso")]
        [Trait(nameof(ItensPedido), "Validar")]
        public void ItensPedido_Validar_DeveTerSucesso()
        {
            //Arrange & Act
            var itensPedido = new ItensPedidoTestBuilder().Build();

            //Assert
            itensPedido.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Quantidade Igual a Zero Deve Falhar")]
        [Trait(nameof(ItensPedido), "Validar")]
        public void Validar_QuantidadeIgualZero_DeveFalhar()
        {
            //Arrange & Act
            var itensPedido = new ItensPedidoTestBuilder()
                .ComQuantidade(0)
                .Build();

            //Assert
            itensPedido.IsFailure.ShouldBeTrue();
            itensPedido.Error.Contains(PedidosConstantes.ItensPedidoQuantidadeDeveSerMaiorQueZero);
        }
        
        [Fact(DisplayName = "Preco Igual a Zero Deve Falhar")]
        [Trait(nameof(ItensPedido), "Validar")]
        public void Validar_PrecoIgualZero_DeveFalhar()
        {
            //Arrange & Act
            var itensPedido = new ItensPedidoTestBuilder()
                .ComPreco(0)
                .Build();

            //Assert
            itensPedido.IsFailure.ShouldBeTrue();
            itensPedido.Error.Contains(PedidosConstantes.ItensPedidoPrecoDeveSerMaiorQueZero);
        }

        [Fact(DisplayName = "Preco Igual a Zero Deve Falhar")]
        [Trait(nameof(ItensPedido), nameof(ItensPedido.AlterarValores))]
        public void AlterarValores_PrecoIgualZero_DeveFalhar()
        {
            //Arrange
            var itensPedido = new ItensPedidoTestBuilder()
                .Build().Value;
            
            //Act
            Result result = itensPedido.AlterarValores(10, 0);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.Contains(PedidosConstantes.ItensPedidoPrecoDeveSerMaiorQueZero);
        }


        [Fact(DisplayName = "Quantidade Igual a Zero Deve Falhar")]
        [Trait(nameof(ItensPedido), nameof(ItensPedido.AlterarValores))]
        public void AlterarValores_QuantidadeIgualZero_DeveFalhar()
        {
            //Arrange 
            var itensPedido = new ItensPedidoTestBuilder()
                .Build().Value;

            //Act
            Result result = itensPedido.AlterarValores(0, 3.99m);


            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.Contains(PedidosConstantes.ItensPedidoPrecoDeveSerMaiorQueZero);
        }

        [Theory(DisplayName = "Valores válidos Deve Alterar Com Sucesso")]
        [Trait(nameof(ItensPedido), nameof(ItensPedido.AlterarValores))]
        [InlineData(1, 3.73, 3.73)]
        [InlineData(2, 6.29, 12.58)]
        [InlineData(5, 5, 25)]
        [InlineData(13, 1.99, 25.87)]
        [InlineData(20, 14.37, 287.4)]
        public void ItensPedido_AlterarValores_DeveAlterarComSucesso(int quantidade, decimal valor, decimal resultado)
        {
            //Arrange 
            var itensPedido = new ItensPedidoTestBuilder()
                .Build();

            //Act
            itensPedido.Value.AlterarValores(quantidade, valor);

            //Assert
            itensPedido.IsSuccess.ShouldBeTrue();
            itensPedido.Value.TotalItem.ShouldBe(resultado);
        }

    }
}

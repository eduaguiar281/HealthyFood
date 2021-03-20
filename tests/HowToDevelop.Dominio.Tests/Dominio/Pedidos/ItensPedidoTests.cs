using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Infraestrutura.Pedidos;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using Shouldly;
using Xunit;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Dominio.Pedidos
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

        [Fact(DisplayName = "Quantidade Null Deve Falhar")]
        [Trait(nameof(ItensPedido), "Validar")]
        public void Validar_QuantidadeNull_DeveFalhar()
        {
            //Arrange & Act
            var itensPedido = new ItensPedidoTestBuilder()
                .ComQuantidade(null)
                .Build();

            //Assert
            itensPedido.IsFailure.ShouldBeTrue();
            itensPedido.Error.Contains(PedidosConstantes.ItensPedidoQuantidadeEhObrigatorio);
        }
        
        [Fact(DisplayName = "Preco Igual a Null Deve Falhar")]
        [Trait(nameof(ItensPedido), "Validar")]
        public void Validar_PrecoNull_DeveFalhar()
        {
            //Arrange & Act
            var itensPedido = new ItensPedidoTestBuilder()
                .ComPreco(null)
                .Build();

            //Assert
            itensPedido.IsFailure.ShouldBeTrue();
            itensPedido.Error.Contains(PedidosConstantes.ItensPedidoPrecoEhObrigatorio);
        }

        [Fact(DisplayName = "Preco Igual a Null Deve Falhar")]
        [Trait(nameof(ItensPedido), nameof(ItensPedido.AlterarValores))]
        public void AlterarValores_PrecoNull_DeveFalhar()
        {
            //Arrange
            var itensPedido = new ItensPedidoTestBuilder()
                .Build().Value;
            
            //Act
            Result result = itensPedido.AlterarValores((Quantidade)10, null);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.Contains(PedidosConstantes.ItensPedidoPrecoEhObrigatorio);
        }


        [Fact(DisplayName = "Quantidade Igual a Null Deve Falhar")]
        [Trait(nameof(ItensPedido), nameof(ItensPedido.AlterarValores))]
        public void AlterarValores_QuantidadeIgualNull_DeveFalhar()
        {
            //Arrange 
            var itensPedido = new ItensPedidoTestBuilder()
                .Build().Value;

            //Act
            Result result = itensPedido.AlterarValores(null, (Preco)3.99m);


            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.Contains(PedidosConstantes.ItensPedidoPrecoEhObrigatorio);
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
            itensPedido.Value.AlterarValores((Quantidade)quantidade, (Preco)valor);

            //Assert
            itensPedido.IsSuccess.ShouldBeTrue();
            itensPedido.Value.TotalItem.Valor.ShouldBe(resultado);
        }

    }
}

using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using CSharpFunctionalExtensions;
using Xunit;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Pedidos
{
    public class PedidoTests
    {
        public PedidoTests()
        {

        }

        [Fact(DisplayName = "Válido Deve Ter Sucesso")]
        [Trait(nameof(Pedido), "Validar")]
        public void Pedido_Validar_DeveTerSucesso()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder().Build();

            //Assert
            pedido.EhValido().IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "GarconId Inválido Deve Falhar")]
        [Trait(nameof(Pedido), "Validar")]
        public void Validar_GarcomIdInvalido_DeveFalhar()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComGarcomId(0)
                .Build();

            //Act
            Result result = pedido.EhValido();
            
            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidosGarcomIdNaoEhValido);
        }

        [Fact(DisplayName = "MesaId Inválido Deve Falhar")]
        [Trait(nameof(Pedido), "Validar")]
        public void Validar_MesaIdInvalido_DeveFalhar()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComMesaId(0)
                .Build();

            //Act
            Result result = pedido.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidosMesaIdNaoEhValido);
        }

        [Fact(DisplayName = "Nome Cliente vazio Deve Falhar")]
        [Trait(nameof(Pedido), "Validar")]
        public void Validar_NomeClienteVazio_DeveFalhar()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComNomeCliente("")
                .Build();

            //Act
            Result result = pedido.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidosNomeClienteEhObrigatorio);
        }

        [Fact(DisplayName = "Nome Cliente Acima Número Caracteres Acima Limite Deve Falhar")]
        [Trait(nameof(Pedido), "Validar")]
        public void Validar_NomeClienteNumeroCaracteresAcimaLimite_DeveFalhar()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComNomeCliente("João da Silva".PadRight(PedidosConstantes.PedidosTamanhoNomeCliente + 5))
                .Build();

            //Act
            Result result = pedido.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidosNomeClienteDeveTerNoMaxmimoNCaracteres);
        }

        [Fact(DisplayName = "Adicionar Item Válido deve ter Sucesso")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_ItemValido_DeveTerSucesso()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComItens(new List<ItensPedido> { ItensPedido.Criar(1, 10, 1.99m).Value })
                .Build();

            //Act
            Result result = pedido.AdicionarItem(2, 1, 1.99m);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(2);
            pedido.Total.ShouldBe(21.89m);
        }

        [Fact(DisplayName = "Adicionar Item Inválido deve ter Sucesso")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_ItemInvalido_DeveTerSucesso()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder()
                .ComItens(new List<ItensPedido> { ItensPedido.Criar(1, 10, 1.99m).Value })
                .Build();

            //Act
            Result result = pedido.AdicionarItem(2, 0, 1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.ShouldBe(19.90m);
        }
    }
}

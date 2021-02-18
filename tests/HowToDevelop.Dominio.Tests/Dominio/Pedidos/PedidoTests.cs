using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using CSharpFunctionalExtensions;
using Xunit;
using HowToDevelop.Core.ObjetosDeValor;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Pedidos
{
    public class PedidoTests
    {
        public PedidoTests()
        {

        }

        [Fact(DisplayName = "Criar Válido Deve Ter Sucesso")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Pedido_Criar_DeveTerSucesso()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder().Build();

            //Assert
            pedido.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Criar com GarconId Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Validar_GarcomIdInvalido_DeveFalhar()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder()
                .ComGarcomId(0)
                .Build();

            //Assert
            pedido.IsFailure.ShouldBeTrue();
            pedido.Error.ShouldContain(PedidosConstantes.PedidosGarcomIdNaoEhValido);
        }

        [Fact(DisplayName = "Criar com MesaId Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Criar_MesaIdInvalido_DeveFalhar()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder()
                .ComMesaId(0)
                .Build();

            //Assert
            pedido.IsFailure.ShouldBeTrue();
            pedido.Error.ShouldContain(PedidosConstantes.PedidosMesaIdNaoEhValido);
        }

        [Fact(DisplayName = "Criar com Nome Cliente vazio Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Criar_NomeClienteVazio_DeveFalhar()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder()
                .ComNomeCliente("")
                .Build();

            //Assert
            pedido.IsFailure.ShouldBeTrue();
            pedido.Error.ShouldContain(PedidosConstantes.PedidosNomeClienteEhObrigatorio);
        }

        [Fact(DisplayName = "Criar com Nome Cliente Acima Número Caracteres Acima Limite Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Criar_NomeClienteNumeroCaracteresAcimaLimite_DeveFalhar()
        {
            //Arrange & Act
            var pedido = new PedidoTestBuilder()
                .ComNomeCliente("João da Silva".PadRight(PedidosConstantes.PedidosTamanhoNomeCliente + 5))
                .Build();

            //Assert
            pedido.IsFailure.ShouldBeTrue();
            pedido.Error.ShouldContain(PedidosConstantes.PedidosNomeClienteDeveTerNoMaxmimoNCaracteres);
        }

        [Fact(DisplayName = "Adicionar Item Válido deve ter Sucesso")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_ItemValido_DeveTerSucesso()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AdicionarItem(2, (Quantidade)1, (Preco)1.99m);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(2);
            pedido.Total.Valor.ShouldBe(21.89m);
        }

        [Fact(DisplayName = "Adicionar Item Inválido deve falhar")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_ItemInvalido_DeveFalhar()
        {
            //Arrange 
            var pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AdicionarItem(2, null, (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.90m);
        }

        [Fact(DisplayName = "Alterar Item Válido Deve ter Sucesso")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_ItemValido_DeveTerSucesso()
        {
            //Arrange
            var pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AlterarItem(0, (Quantidade)10, (Preco)2.99m);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(29.90m);
        }

        [Fact(DisplayName = "Alterar Item inválido Deve falhar")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_ItemInvalido_DeveFalhar()
        {
            //Arrange
            var pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AlterarItem(0, null, (Preco)2.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.ItensPedidoQuantidadeEhObrigatorio);
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.90m);
        }
       
        [Fact(DisplayName = "Alterar Item Inexsitente Deve falhar")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_ItemInexistente_DeveFalhar()
        {
            //Arrange
            var pedido = new PedidoTestBuilder().Build().Value;
            int idItem = 1;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AlterarItem(idItem, null, (Preco)2.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, idItem));
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.90m);
        }

        [Fact(DisplayName = "Remover Item Id Válido Deve ter Sucesso")]
        [Trait(nameof(Pedido), "RemoverItem")]
        public void RemoverItem_IdValido_DeveTerSucesso()
        {
            //Arrange
            var pedido = new PedidoTestBuilder().Build().Value;
            int idItem = 0;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.RemoverItem(idItem);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(0);
            pedido.Total.Valor.ShouldBe(0);
        }

        [Fact(DisplayName = "Remover Item Id Inválido Deve Falhar")]
        [Trait(nameof(Pedido), "RemoverItem")]
        public void RemoverItem_IdInvalido_DeveFalhar()
        {
            //Arrange
            var pedido = new PedidoTestBuilder().Build().Value;
            int idItem = 1;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.RemoverItem(idItem);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, idItem));
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.90m);
        }

    }
}

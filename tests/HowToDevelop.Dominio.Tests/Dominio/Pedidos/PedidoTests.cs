using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using CSharpFunctionalExtensions;
using Xunit;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;
using System;

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
            Result<Pedido> pedido = new PedidoTestBuilder().Build();

            //Assert
            pedido.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Criar com GarconId Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.Criar))]
        public void Validar_GarcomIdInvalido_DeveFalhar()
        {
            //Arrange & Act
            Result<Pedido> pedido = new PedidoTestBuilder()
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
            Result<Pedido> pedido = new PedidoTestBuilder()
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
            Result<Pedido> pedido = new PedidoTestBuilder()
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
            Result<Pedido> pedido = new PedidoTestBuilder()
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
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AdicionarItem(2, (Quantidade)1, (Preco)1.99m);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(2);
            pedido.Total.Valor.ShouldBe(21.89m);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

        [Fact(DisplayName = "Adicionar Item Inválido deve falhar")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_ItemInvalido_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, (Quantidade)10, (Preco)1.99m);

            //Act
            Result result = pedido.AdicionarItem(2, null, (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.90m);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

        [Fact(DisplayName = "Adicionar Primeiro Item não é válido deve falhar")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_PrimeiroItemEhInvalido_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;

            //Act
            Result result = pedido.AdicionarItem(2, null, (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            pedido.Itens.Count.ShouldBe(0);
            pedido.Total.Valor.ShouldBe(0);
            pedido.Status.ShouldBe(StatusPedido.Novo);
        }

        [Fact(DisplayName = "Adicionar Pedido Fechado deve falhar")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_PedidoFechado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.FecharPedido(5, 0, 0, TipoDesconto.Valor);

            //Act
            Result result = pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoFechadoNaoPodeSerAlterado);
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.9m);
            pedido.Status.ShouldBe(StatusPedido.Fechado);
        }

        [Fact(DisplayName = "Adicionar Pedido Cancelado deve falhar")]
        [Trait(nameof(Pedido), "AdicionarItem")]
        public void AdicionarItem_PedidoCancelado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.CancelarPedido();

            //Act
            Result result = pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoCanceladoNaoPodeSerAlterado);
            pedido.Status.ShouldBe(StatusPedido.Cancelado);
        }


        [Fact(DisplayName = "Alterar Item Válido Deve ter Sucesso")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_ItemValido_DeveTerSucesso()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
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
            Pedido pedido = new PedidoTestBuilder().Build().Value;
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
            Pedido pedido = new PedidoTestBuilder().Build().Value;
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


        [Fact(DisplayName = "Alterar Item, Quando pedido é Cancelado deve falhar")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_PedidoCancelado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.CancelarPedido();

            //Act
            Result result = pedido.AlterarItem(2, new Quantidade(1), (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoCanceladoNaoPodeSerAlterado);
            pedido.Status.ShouldBe(StatusPedido.Cancelado);
        }

        [Fact(DisplayName = "Alterar Item, Quando Pedido é Fechado deve falhar")]
        [Trait(nameof(Pedido), "AlterarItem")]
        public void AlterarItem_PedidoFechado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.FecharPedido(5, 0, 0, TipoDesconto.Valor);

            //Act
            Result result = pedido.AlterarItem(2, new Quantidade(1), (Preco)1.99m);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoFechadoNaoPodeSerAlterado);
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.9m);
            pedido.Status.ShouldBe(StatusPedido.Fechado);
        }

        [Fact(DisplayName = "Remover Item Id Válido Deve ter Sucesso")]
        [Trait(nameof(Pedido), "RemoverItem")]
        public void RemoverItem_IdValido_DeveTerSucesso()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
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
            Pedido pedido = new PedidoTestBuilder().Build().Value;
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

        [Fact(DisplayName = "Remover Item Pedido Fechado deve falhar")]
        [Trait(nameof(Pedido), "RemoverItem")]
        public void RemoverItem_PedidoFechado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.FecharPedido(5, 0, 0, TipoDesconto.Valor);

            //Act
            Result result = pedido.RemoverItem(1);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoFechadoNaoPodeSerAlterado);
            pedido.Itens.Count.ShouldBe(1);
            pedido.Total.Valor.ShouldBe(19.9m);
            pedido.Status.ShouldBe(StatusPedido.Fechado);
        }

        [Fact(DisplayName = "Remover Item Pedido Cancelado deve falhar")]
        [Trait(nameof(Pedido), "RemoverItem")]
        public void RemoverItem_PedidoCancelado_DeveFalhar()
        {
            //Arrange 
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(2, new Quantidade(10), (Preco)1.99m);
            pedido.CancelarPedido();

            //Act
            Result result = pedido.RemoverItem(1);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoCanceladoNaoPodeSerAlterado);
            pedido.Status.ShouldBe(StatusPedido.Cancelado);
        }

        [Fact(DisplayName = "Cancelar Pedido Deve Ter Sucesso")]
        [Trait(nameof(Pedido), nameof(Pedido.CancelarPedido))]
        public void Pedido_Cancelar_DeveTerSucesso()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;

            //Act
            var result = pedido.CancelarPedido();

            //Assert
            result.IsSuccess.ShouldBeTrue();
            pedido.Comissao.ShouldBeNull();
            pedido.Desconto.ShouldBeNull();
        }

        [Fact(DisplayName = "Cancelar Pedido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.CancelarPedido))]
        public void Cancelar_PedidoJaCancelado_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;

            //Act
            pedido.CancelarPedido();
            var result = pedido.CancelarPedido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoJaFoiCancelado);
        }

        [Theory(DisplayName = "Fechar Pedido Deve Ter Sucesso")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        [InlineData(100, 5, TipoDesconto.Percentual, 3, 5, 98)]
        [InlineData(200, 0, TipoDesconto.Valor, 10, 5, 210)]
        [InlineData(45.25, 5, TipoDesconto.Percentual, 5, 5, 47.99)]
        [InlineData(350.65, 30, TipoDesconto.Valor, 15, 5, 335.65)]
        public void FecharPedido_ValoresValidos_DeveTerSucesso(decimal valorItem, decimal desconto, TipoDesconto tipo, decimal gorjeta, decimal comissao, decimal totalEsperado)
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(valorItem));

            //Act
            Result result = pedido.FecharPedido(comissao, gorjeta, desconto, tipo);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            Math.Round(pedido.Total, 2).ShouldBe(totalEsperado);
            pedido.Status.ShouldBe(StatusPedido.Fechado);
        }

        [Fact(DisplayName = "Fechar Pedido Status Diferente de Em Andamento Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        public void FecharPedido_StatusDiferenteEmAndamento_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(100));
            pedido.CancelarPedido();

            //Act
            Result result = pedido.FecharPedido(5, 0, 0, TipoDesconto.Valor);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PedidoDeveEstarEmAndamento);
            pedido.Status.ShouldBe(StatusPedido.Cancelado);
        }

        [Fact(DisplayName = "Fechar Pedido Percentual Desconto Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        public void FecharPedido_PercentualDescontoInvalido_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(100));

            //Act
            Result result = pedido.FecharPedido(0, 0, 0, TipoDesconto.Percentual);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(DescontoConstantes.PercentualDeveSerMaiorQueZero);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

        [Fact(DisplayName = "Fechar Pedido Valor Desconto Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        public void FecharPedido_ValorDescontoInvalido_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(100));

            //Act
            Result result = pedido.FecharPedido(0, 0, -5, TipoDesconto.Valor);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(DescontoConstantes.ValorDescontoNaoPodeSerMenorQueZero);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

        [Fact(DisplayName = "Fechar Pedido Percentual Desconto Acima do Limite Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        public void FecharPedido_PercentualDescontoAcimaLimite_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(100));

            //Act
            Result result = pedido.FecharPedido(0, 0, (PedidosConstantes.PercentualMaximo + 0.1m)*100, TipoDesconto.Percentual);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PedidosConstantes.PercentualDescontoNaoDeveUltrapassarPercentualMaximo);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

        [Fact(DisplayName = "Fechar Pedido Valor Comissão Inválido Deve Falhar")]
        [Trait(nameof(Pedido), nameof(Pedido.FecharPedido))]
        public void FecharPedido_ValorComissaoInvalido_DeveFalhar()
        {
            //Arrange
            Pedido pedido = new PedidoTestBuilder().Build().Value;
            pedido.AdicionarItem(1, new Quantidade(1), new Preco(100));

            //Act
            Result result = pedido.FecharPedido(-1, 0, 0, TipoDesconto.Valor);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(PercentualConstantes.PercentualNaoDeveSerMenorQueZero);
            pedido.Status.ShouldBe(StatusPedido.EmAndamento);
        }

    }
}

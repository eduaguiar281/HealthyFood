using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using CSharpFunctionalExtensions;
using Xunit;
using HowToDevelop.HealthFood.Infraestrutura.Produtos;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using HowToDevelop.Core.ObjetosDeValor;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Dominio.Produtos
{
    public class ProdutoTests
    {
        public ProdutoTests()
        {

        }

        [Fact(DisplayName = "Criar Tipo Bebida Válido Deve Ter Sucesso")]
        [Trait(nameof(Produto), "Criar")]
        public void Produto_CriarTipoBebida_DeveTerSucesso()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComTipo(TipoProduto.Bebida)
                .Build();

            //Assert
            produto.IsSuccess.ShouldBeTrue();
            produto.Value.TipoProduto.ShouldBe(TipoProduto.Bebida);
        }

        [Fact(DisplayName = "Criar Tipo Lanche Válido Deve Ter Sucesso")]
        [Trait(nameof(Produto), "Criar")]
        public void Produto_CriarTipoLanche_DeveTerSucesso()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComTipo(TipoProduto.Lanche)
                .Build();

            //Assert
            produto.IsSuccess.ShouldBeTrue();
            produto.Value.TipoProduto.ShouldBe(TipoProduto.Lanche);
        }

        [Fact(DisplayName = "Criar Tipo Lanche Válido Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Produto_CriarTipoLanche_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComTipo(TipoProduto.Lanche)
                .ComPreco(null)
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoPrecoNaoFoiInformado);
        }


        [Fact(DisplayName = "Criar Tipo Lanche Válido Deve Ter Sucesso")]
        [Trait(nameof(Produto), "Criar")]
        public void Produto_CriarTipoOutros_DeveTerSucesso()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComTipo(TipoProduto.Outros)
                .Build();

            //Assert
            produto.IsSuccess.ShouldBeTrue();
            produto.Value.TipoProduto.ShouldBe(TipoProduto.Outros);
        }

        [Fact(DisplayName = "Criar Tipo Outros Válido Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Produto_CriarTipoOutros_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComTipo(TipoProduto.Outros)
                .ComPreco(null)
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoPrecoNaoFoiInformado);
        }


        [Fact(DisplayName = "Sem Codigo Barras Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Criar_SemCodigoBarras_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComCodigoBarras("")
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoCodigoBarrasEhObrigatorio);
        }

        [Fact(DisplayName = "Código de Barras Acima do Limite de Caracteres Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Criar_CodigoBarrasAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComCodigoBarras("78912313030969".PadRight(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras + 5))
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres);
        }

        [Fact(DisplayName = "Sem Descricao Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Criar_SemDescricao_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComDescricao("")
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoDescricaoEhObrigatorio);
        }

        [Fact(DisplayName = "Descrição Acima do Limite de Caracteres Deve Falhar")]
        [Trait(nameof(Produto), "Criar")]
        public void Criar_DescricaoAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder()
                .ComDescricao("Coca-Cola".PadRight(ProdutosConstantes.ProdutoTamanhoCampoDescricao + 5))
                .Build();

            //Assert
            produto.IsFailure.ShouldBeTrue();
            produto.Error.ShouldContain(ProdutosConstantes.ProdutoDescricaoDeveTerNoMaximoNCaracteres);
        }

        [Fact(DisplayName = "Alteração Valores Validos Deve Ter Sucesso")]
        [Trait(nameof(Produto), nameof(Produto.AlterarDados))]
        public void AlterarDados_ValoresValidos_DeveTerSucesso()
        {
            //Arrange 
            var produto = new ProdutoTestBuilder()
                .Build().Value;
            string novoCodigoBarras = "7891112031968";
            string novaDescricao = "Fanta Laranja";
            Preco novoPreco = new Preco(2.99m);

            //Act
            Result result = produto.AlterarDados(novoCodigoBarras, novaDescricao, novoPreco);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            produto.CodigoBarras.ShouldBe(novoCodigoBarras);
            produto.Preco.ShouldBe(novoPreco);
            produto.Descricao.ShouldBe(novaDescricao);
        }

        [Fact(DisplayName = "Alteração Valores Inválidos Deve Falhar")]
        [Trait(nameof(Produto), nameof(Produto.AlterarDados))]
        public void AlterarDados_ValoresInvalidos_DeveFalhar()
        {
            //Arrange 
            var produto = new ProdutoTestBuilder()
                .Build().Value;
            string novoCodigoBarras = "7891112031968";
            string novaDescricao = "Fanta Laranja";
            Preco novoPreco = null;

            //Act
            Result result = produto.AlterarDados(novoCodigoBarras, novaDescricao, novoPreco);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoPrecoNaoFoiInformado);
            produto.CodigoBarras.ShouldNotBe(novaDescricao);
            produto.Preco.ShouldNotBe(novoPreco);
            produto.Descricao.ShouldNotBe(novaDescricao);
        }

    }
}

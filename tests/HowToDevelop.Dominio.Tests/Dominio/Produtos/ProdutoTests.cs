using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using CSharpFunctionalExtensions;
using Xunit;
using HowToDevelop.HealthFood.Dominio.Produtos;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Produtos
{
    public class ProdutoTests
    {
        public ProdutoTests()
        {

        }

        [Fact(DisplayName = "Válido Deve Ter Sucesso")]
        [Trait(nameof(Produto), "Validar")]
        public void Produto_Validar_DeveTerSucesso()
        {
            //Arrange & Act
            var produto = new ProdutoTestBuilder().Build();

            //Assert
            produto.EhValido().IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Sem Codigo Barras Deve Falhar")]
        [Trait(nameof(Produto), "Validar")]
        public void Validar_SemCodigoBarras_DeveFalhar()
        {
            //Arrange
            var produto = new ProdutoTestBuilder()
                .ComCodigoBarras("")
                .Build();

            //Act 
            Result result = produto.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoCodigoBarrasEhObrigatorio);
        }

        [Fact(DisplayName = "Código de Barras Acima do Limite de Caracteres Deve Falhar")]
        [Trait(nameof(Produto), "Validar")]
        public void Validar_CodigoBarrasAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange
            var produto = new ProdutoTestBuilder()
                .ComCodigoBarras("78912313030969".PadRight(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras + 5))
                .Build();

            //Act 
            Result result = produto.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres);
        }

        [Fact(DisplayName = "Sem Descricao Deve Falhar")]
        [Trait(nameof(Produto), "Validar")]
        public void Validar_SemDescricao_DeveFalhar()
        {
            //Arrange
            var produto = new ProdutoTestBuilder()
                .ComDescricao("")
                .Build();

            //Act 
            Result result = produto.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoDescricaoEhObrigatorio);
        }

        [Fact(DisplayName = "Descrição Acima do Limite de Caracteres Deve Falhar")]
        [Trait(nameof(Produto), "Validar")]
        public void Validar_DescricaoAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange
            var produto = new ProdutoTestBuilder()
                .ComDescricao("Coca-Cola".PadRight(ProdutosConstantes.ProdutoTamanhoCampoDescricao + 5))
                .Build();

            //Act 
            Result result = produto.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoDescricaoDeveTerNoMaximoNCaracteres);
        }

        [Theory(DisplayName = "Preço Inferior ou igual a Zero Deve Falhar")]
        [Trait(nameof(Produto), "Validar")]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validar_PrecoInferiorZero_DeveFalhar(decimal preco)
        {
            //Arrange
            var produto = new ProdutoTestBuilder()
                .ComPreco(preco)
                .Build();

            //Act 
            Result result = produto.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoPrecoNaoPodeSerMenorOuIgualZero);
        }
    }
}

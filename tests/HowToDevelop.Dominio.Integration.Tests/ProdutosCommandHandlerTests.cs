using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Dominio.Integration.Tests.Fixtures;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Produtos;
using HowToDevelop.HealthFood.Produtos.Application.Commands;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
using HowToDevelop.HealthFood.Produtos.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HowToDevelop.Dominio.Integration.Tests
{
    public class ProdutosCommandHandlerTests : IAssemblyFixture<ProdutoTestFixture>, IAssemblyFixture<HealthFoodDbContextTestFixture>
    {
        private readonly ProdutoTestFixture _produtoFixture;
        private readonly HealthFoodDbContextTestFixture _fixture;

        public ProdutosCommandHandlerTests(ProdutoTestFixture produtoFixture, HealthFoodDbContextTestFixture fixture)
        {
            _produtoFixture = produtoFixture;
            _fixture = fixture;
        }

        [Fact(DisplayName = "ProdutosCommandHandler Incluir Produto Deve Ter Sucesso"), Order(1)]
        [Trait(nameof(ProdutosCommandHandler), nameof(IncluirProdutoCommand))]
        public async Task ProdutosCommandHandler_IncluirProdutoCommand_DeveTerSucesso()
        {
            // Arrange
            var command = new IncluirProdutoCommand(ProdutoTestFixture.CodigoBarrasProdutoIncluido,
                ProdutoTestFixture.DescricaoProdutoIncluido, ProdutoTestFixture.PrecoProdutoIncluido,
                ProdutoTestFixture.TipoProdutoIncluido);
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new ProdutosCommandHandler(new ProdutosRepositorio(context));

            // Act
            Result<ProdutoDto> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _produtoFixture.ProdutoIdIncluido = result.Value.Id;
            result.Value.CodigoBarras.ShouldBe(command.CodigoBarras);
            result.Value.Descricao.ShouldBe(command.Descricao);
            result.Value.Preco.ShouldBe(command.Preco);
            result.Value.TipoProduto.ShouldBe(command.TipoProduto);
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Produto produtoAssertion = await contextAssertion.Produtos
                .FirstOrDefaultAsync(s => s.Id == _produtoFixture.ProdutoIdIncluido);
            produtoAssertion.ShouldNotBeNull();
            produtoAssertion.Descricao.ShouldBe((Descricao.Criar(ProdutoTestFixture.DescricaoProdutoIncluido).Value));
            produtoAssertion.CodigoBarras.ShouldBe(ProdutoTestFixture.CodigoBarrasProdutoIncluido);
            produtoAssertion.Preco.ShouldBe(new Preco(ProdutoTestFixture.PrecoProdutoIncluido));
            produtoAssertion.TipoProduto.ShouldBe(ProdutoTestFixture.TipoProdutoIncluido);
        }

        [Fact(DisplayName = "ProdutosCommandHandler Alterar Dados do Produto Deve Ter Sucesso"), Order(2)]
        [Trait(nameof(ProdutosCommandHandler), nameof(AlterarDadosProdutoCommand))]
        public async Task ProdutosCommandHandler_AlterarDadosProdutoCommand_DeveTerSucesso()
        {
            // Arrange
            var command = new AlterarDadosProdutoCommand(_produtoFixture.ProdutoIdIncluido, 
                ProdutoTestFixture.CodigoBarrasProdutoAlterado,
                ProdutoTestFixture.DescricaoProdutoAlterado, ProdutoTestFixture.PrecoProdutoAlterado);
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new ProdutosCommandHandler(new ProdutosRepositorio(context));

            // Act
            Result<ProdutoDto> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.CodigoBarras.ShouldBe(command.CodigoBarras);
            result.Value.Descricao.ShouldBe(command.Descricao);
            result.Value.Preco.ShouldBe(command.Preco);
            result.Value.TipoProduto.ShouldBe(ProdutoTestFixture.TipoProdutoIncluido);
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Produto produtoAssertion = await contextAssertion.Produtos
                .FirstOrDefaultAsync(s => s.Id == _produtoFixture.ProdutoIdIncluido);
            produtoAssertion.ShouldNotBeNull();
            produtoAssertion.Descricao.ShouldBe((Descricao.Criar(ProdutoTestFixture.DescricaoProdutoAlterado).Value));
            produtoAssertion.CodigoBarras.ShouldBe(ProdutoTestFixture.CodigoBarrasProdutoAlterado);
            produtoAssertion.Preco.ShouldBe(new Preco(ProdutoTestFixture.PrecoProdutoAlterado));
            produtoAssertion.TipoProduto.ShouldBe(ProdutoTestFixture.TipoProdutoIncluido);
        }

        [Fact(DisplayName = "ProdutosCommandHandler Excluir Produto Deve Ter Sucesso"), Order(3)]
        [Trait(nameof(ProdutosCommandHandler), nameof(ExcluirProdutoCommand))]
        public async Task ProdutosCommandHandler_ExcluirProdutoCommand_DeveTerSucesso()
        {
            // Arrange
            var command = new ExcluirProdutoCommand(_produtoFixture.ProdutoIdIncluido);
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new ProdutosCommandHandler(new ProdutosRepositorio(context));

            // Act
            Result<int> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(_produtoFixture.ProdutoIdIncluido);
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Produto produtoAssertion = await contextAssertion.Produtos
                .FirstOrDefaultAsync(s => s.Id == _produtoFixture.ProdutoIdIncluido);
            produtoAssertion.ShouldBeNull();
        }
    }
}

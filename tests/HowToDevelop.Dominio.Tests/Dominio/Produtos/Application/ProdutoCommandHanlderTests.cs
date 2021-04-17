using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Produtos;
using HowToDevelop.HealthFood.Produtos.Application.Commands;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Produtos.Application
{
    [Collection(nameof(ProdutosHandlersFixtureCollection))]
    public class ProdutoCommandHanlderTests 
    {
        private readonly ProdutosHandlersFixture _fixture;
        public ProdutoCommandHanlderTests(ProdutosHandlersFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = "Incluir Produto Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(IncluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        [InlineData(TipoProduto.Bebida, "Suco natural de laranja", 3.99)]
        [InlineData(TipoProduto.Lanche, "Sanduiche Natural de Atum", 6.5)]
        [InlineData(TipoProduto.Outros, "Barra de Cereal", 1.99)]
        public async Task IncluirProdutoCommand_ComandoValido_DeveTerSucesso(TipoProduto tipo, string descricao, decimal preco)
        {
            // Arrange
            var command = new IncluirProdutoCommand("7891213030969", descricao, preco, tipo);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Produto>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.Descricao.ShouldBe(descricao);
            result.Value.CodigoBarras.ShouldBe(command.CodigoBarras);
            result.Value.Preco.ShouldBe(command.Preco);
            result.Value.TipoProduto.ShouldBe(tipo);
        }

        [Fact(DisplayName = "Incluir Produto Comando Invalido Deve Falhar")]
        [Trait(nameof(IncluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task IncluirProdutoCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            var command = new IncluirProdutoCommand("7891213030969", string.Empty, 1, TipoProduto.Bebida);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(DescricaoConstantes.DescricaoEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Produto>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Incluir Produto Banco de Dados Nao Inclui Deve Falhar")]
        [Trait(nameof(IncluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task IncluirProdutoCommand_BancoDeDadosNaoInclui_DeveTerSucesso()
        {
            // Arrange
            var command = new IncluirProdutoCommand("7891213030969", "Suco de Uva Natural", 1, TipoProduto.Bebida);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoNaoFoiPossivelSalvar);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Produto>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Alterar Dados do Produto Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(AlterarDadosProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task AlterarDadosProdutoCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            int idProduto = 1;
            var produto = Produto.CriarTipoBebida("7891213030969", "Suco de Maracujá", new Preco(5.99m), idProduto).Value;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult(produto));
            var command = new AlterarDadosProdutoCommand(idProduto, "7891213030968", "Suco de Maracujá com leite", 6.99m);

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.CodigoBarras.ShouldBe(command.CodigoBarras);
            result.Value.Descricao.ShouldBe(command.Descricao);
            result.Value.Preco.ShouldBe(command.Preco);
            result.Value.Id.ShouldBe(idProduto);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(produto), Times.Once);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Alterar Dados do Produto Comando Inválido Deve Falhar")]
        [Trait(nameof(AlterarDadosProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task AlterarDadosProdutoCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            int idProduto = 1;
            var produto = Produto.CriarTipoBebida("7891213030969", "Suco de Maracujá", new Preco(5.99m), idProduto).Value;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult(produto));
            var command = new AlterarDadosProdutoCommand(idProduto, "7891213030968", string.Empty, 6.99m);

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(DescricaoConstantes.DescricaoEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(produto), Times.Never);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Alterar Dados do Produto Produto não encontrado Deve Falhar")]
        [Trait(nameof(AlterarDadosProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task AlterarDadosProdutoCommand_ProdutoNaoEncontrado_DeveFalhar()
        {
            // Arrange
            int idProduto = 1;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult<Produto>(null));
            var command = new AlterarDadosProdutoCommand(idProduto, "7891213030968", "Suco de Maracujá com leite", 6.99m);

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(ProdutosConstantes.ProdutoNaoFoiEncontradoComIdInformado, idProduto));
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Produto>()), Times.Never);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact(DisplayName = "Alterar Dados do Produto Banco de Dados Nao Salva Deve Falhar")]
        [Trait(nameof(AlterarDadosProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task AlterarDadosProdutoCommand_BancoDeDadoNaoSalva_DeveFalhar()
        {
            // Arrange
            int idProduto = 1;
            var produto = Produto.CriarTipoBebida("7891213030969", "Suco de Maracujá", new Preco(5.99m), idProduto).Value;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult(produto));
            var command = new AlterarDadosProdutoCommand(idProduto, "7891213030968", "Suco de Maracujá com leite", 6.99m);

            // Act
            Result<ProdutoDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoNaoFoiPossivelSalvar);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(produto), Times.Once);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact(DisplayName = "Excluir Produto Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(ExcluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task ExcluirProdutoCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            int idProduto = 1;
            var produto = Produto.CriarTipoBebida("7891213030969", "Suco de Laranja", new Preco(3.99m), idProduto).Value;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult(produto));
            var command = new ExcluirProdutoCommand(idProduto);

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(idProduto);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(produto), Times.Once);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Excluir Produto Banco Nao Salva Deve Falhar")]
        [Trait(nameof(ExcluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task ExcluirProdutoCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            int idProduto = 1;
            var produto = Produto.CriarTipoBebida("7891213030969", "Suco de Laranja", new Preco(3.99m), idProduto).Value;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult(produto));
            var command = new ExcluirProdutoCommand(idProduto);

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(ProdutosConstantes.ProdutoNaoFoiPossivelExcluir);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(produto), Times.Once);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Excluir Produto Produto Id Nao Localizado Deve Falhar")]
        [Trait(nameof(ExcluirProdutoCommand), nameof(ProdutosCommandHandler.Handle))]
        public async Task ExcluirProdutoCommand_ProdutoIdNaoLocalizado_DeveFalhar()
        {
            // Arrange
            int idProduto = 1;
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));
            _fixture.RepositorioSetup(r => r.ObterPorIdAsync(idProduto))
                .Returns(Task.FromResult<Produto>(null));
            var command = new ExcluirProdutoCommand(idProduto);

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(ProdutosConstantes.ProdutoNaoFoiEncontradoComIdInformado, idProduto));
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(idProduto), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(It.IsAny<Produto>()), Times.Never);
            _fixture.UnitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Garcons.Application.Commands;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Garcons.Application
{
    [Collection(nameof(GarconsHandlersFixtureCollection))]
    public class GarconsCommandHandlerTests
    {
        private readonly GarconsHandlersFixture _fixture;
        public GarconsCommandHandlerTests(GarconsHandlersFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Incluir Garcom Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(IncluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task IncluirGarconCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            var command = new IncluirGarcomCommand("José da Silva", "Zé");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Garcom>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Apelido.ShouldBe(command.Apelido);
        }

        [Fact(DisplayName = "Incluir Garcom Comando inválido Deve Falhar")]
        [Trait(nameof(IncluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task IncluirGarconCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            var command = new IncluirGarcomCommand("", "Zé");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Garcom>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Incluir Garcom. Banco não salva Deve Falhar")]
        [Trait(nameof(IncluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task IncluirGarconCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            var command = new IncluirGarcomCommand("José da Silva", "Zé");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Garcom>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}

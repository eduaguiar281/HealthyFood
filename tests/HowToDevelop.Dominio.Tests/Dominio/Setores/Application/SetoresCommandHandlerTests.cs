using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Setores.Application.Commands;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using HowToDevelop.HealthFood.Setores;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.Dominio.Tests.Dominio.Setores.Application
{
    [Collection(nameof(SetoresHandlersFixtureCollection))]
    public class SetoresCommandHandlerTests
    {
        private readonly SetoresHandlersFixture _fixture;
        public SetoresCommandHandlerTests(SetoresHandlersFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Incluir Setor Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(IncluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task IncluirSetorCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            var command = new IncluirSetorCommand("Área VIP", "V01");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Setor>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Sigla.ShouldBe(command.Sigla);
        }

        [Fact(DisplayName = "Incluir Setor Comando Dados Inválido Deve Falhar")]
        [Trait(nameof(IncluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task IncluirSetorCommand_ComandoDadosInvalido_DeveFalhar()
        {
            // Arrange
            var command = new IncluirSetorCommand("", "V01");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Incluir Setor Erro Banco Dados Deve Falhar")]
        [Trait(nameof(IncluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task IncluirSetorCommand_ErroBancoDados_DeveFalhar()
        {
            // Arrange
            var command = new IncluirSetorCommand("Área Vip", "V01");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NaoFoiPossivelSalvarSetor);
            _fixture.Repositorio.Verify(r => r.Adicionar(It.IsAny<Setor>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Alterar Descrição Setor Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(AlterarDescricaoSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AlterarDescricaoSetorCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new AlterarDescricaoSetorCommand(setor.Id, "Área VIP", "V01");

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Sigla.ShouldBe(command.Sigla);

        }

        [Fact(DisplayName = "Alterar Descrição Setor Id do Setor Inválido Deve Falhar")]
        [Trait(nameof(AlterarDescricaoSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AlterarDescricaoSetorCommand_SetorIdInvalido_DeveFalhar()
        {
            // Arrange
            int setorId = 1;
            var command = new AlterarDescricaoSetorCommand(setorId, "Área VIP", "V01");

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setorId))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, setorId));
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Alterar Descrição, Dados Inválidos Deve Falhar")]
        [Trait(nameof(AlterarDescricaoSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AlterarDescricaoSetorCommand_DadosInvalidos_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new AlterarDescricaoSetorCommand(setor.Id, "", "V01");

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Alterar Descrição, Banco não grava. Deve Falhar")]
        [Trait(nameof(AlterarDescricaoSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AlterarDescricaoSetorCommand_BancoNaoGrava_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new AlterarDescricaoSetorCommand(setor.Id, "Área VIP", "V01");

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<SetorDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NaoFoiPossivelSalvarSetor);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Excluir Setor Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(ExcluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task ExcluirSetorCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new ExcluirSetorCommand(setor.Id);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.ShouldBe(command.SetorId);
        }

        [Fact(DisplayName = "Excluir Setor Setor Id Inválido Deve Falhar")]
        [Trait(nameof(ExcluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task ExcluirSetorCommand_SetorIdInvalido_DeveFalhar()
        {
            // Arrange
            int setorId = 1;
            var command = new ExcluirSetorCommand(setorId);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setorId))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, setorId));
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Excluir Setor, Banco Nao Grava Deve Falhar")]
        [Trait(nameof(ExcluirSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task ExcluirSetorCommand_BancoNaoGrava_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new ExcluirSetorCommand(setor.Id);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NaoFoiPossivelRemoverSetor);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(It.IsAny<Setor>()), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact(DisplayName = "Adicionar Mesa Setor Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(AdicionarMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AdicionarMesaSetorCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new AdicionarMesaSetorCommand(setor.Id, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.FirstOrDefault(m => m.Numeracao == command.Numeracao).ShouldNotBeNull();
        }

        [Fact(DisplayName = "Adicionar Mesa, Setor Id inválido. Deve Falhar")]
        [Trait(nameof(AdicionarMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AdicionarMesaSetorCommand_SetorIdInvalido_DeveFalhar()
        {
            // Arrange
            int setorId = 1;
            var command = new AdicionarMesaSetorCommand(setorId, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setorId))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, setorId));
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Adicionar Mesa, Banco de Dados Nao Atualiza. Deve Falhar")]
        [Trait(nameof(AdicionarMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AdicionarMesaSetorCommand_BancoDadosNaoAtualiza_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new AdicionarMesaSetorCommand(setor.Id, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NaoFoiPossivelRemoverMesaSetor);
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Mesa, comando inválido Deve Falhar")]
        [Trait(nameof(AdicionarMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task AdicionarMesaSetorCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            var command = new AdicionarMesaSetorCommand(setor.Id, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.JaExisteUmaMesaComEstaNumeracaoParaSetor, setor.Id));
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Remover Mesa, Comando Válido. Deve Ter Sucesso")]
        [Trait(nameof(RemoverMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task RemoverMesaSetorCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new RemoverMesaSetorCommand(setor.Id, 1);
            setor.AdicionarMesa(command.Numeracao);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            setor.Mesas.FirstOrDefault(m => m.Numeracao == command.Numeracao).ShouldBeNull();
        }

        [Fact(DisplayName = "Remover Mesa, Setor Id Inválido. Deve Falhar")]
        [Trait(nameof(RemoverMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task RemoverMesaSetorCommand_SetorIdInvalido_DeveFalhar()
        {
            // Arrange
            int setorId = 1;
            var command = new RemoverMesaSetorCommand(setorId, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setorId))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, setorId));
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Setor>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Remover Mesa, Comando inválido. Deve Falhar")]
        [Trait(nameof(RemoverMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task RemoverMesaSetorCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new RemoverMesaSetorCommand(setor.Id, 1);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.MesaInformadaNaoFoiLocalizada, command.Numeracao));
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Remover Mesa, Banco de Dados Nao Atualiza. Deve Falhar")]
        [Trait(nameof(RemoverMesaSetorCommand), nameof(SetoresCommandHandler.Handle))]
        public async Task RemoverMesaSetorCommand_BancoDadosNaoAtualiza_DeveFalhar()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            var command = new RemoverMesaSetorCommand(setor.Id, 1);
            setor.AdicionarMesa(command.Numeracao);

            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<IEnumerable<MesaDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NaoFoiPossivelRemoverMesaSetor);
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(setor), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            
        }

    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Dominio.Tests.Builders;
using HowToDevelop.HealthFood.Garcons;
using HowToDevelop.HealthFood.Garcons.Application.Commands;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using Moq;
using Shouldly;
using System.Collections.Generic;
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

        [Fact(DisplayName = "Alterar Dados Pessoais Garcom. Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(AlterarDadosPessoaisGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task AlterarDadosPessoaisGarcomCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var command = new AlterarDadosPessoaisGarcomCommand(garcom.Id, "Eduardo da Silva", "Edu");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcom.Id))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Apelido.ShouldBe(command.Apelido);
        }

        [Fact(DisplayName = "Alterar Dados Pessoais Garcom. Comando Inválido Deve Falhar")]
        [Trait(nameof(AlterarDadosPessoaisGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task AlterarDadosPessoaisGarcomCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var command = new AlterarDadosPessoaisGarcomCommand(garcom.Id, "", "Garçom");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcom.Id))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Alterar Dados Pessoais Garcom. Banco Não Salva Deve Falhar")]
        [Trait(nameof(AlterarDadosPessoaisGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task AlterarDadosPessoaisGarcomCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var command = new AlterarDadosPessoaisGarcomCommand(garcom.Id, "Eduardo da Silva", "Edu");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcom.Id))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Alterar Dados Pessoais Garcom. Id não encontrado Deve Falhar")]
        [Trait(nameof(AlterarDadosPessoaisGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task AlterarDadosPessoaisGarcomCommand_IdNaoEncontrado_DeveFalhar()
        {
            // Arrange
            int garcomId = 1;
            var command = new AlterarDadosPessoaisGarcomCommand(garcomId, "Eduardo da Silva", "Edu");
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcomId))
                .Returns(Task.FromResult<Maybe<Garcom>>(null));

            // Act
            Result<GarcomDto> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, garcomId));
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Garcom>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Vincular Setor ao Garcom. Comando Válido Deve Ter Sucesso")]
        [Trait(nameof(VincularSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task VincularSetorGarcomCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new VincularSetorGarcomCommand(garcom.Id, builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            _fixture.Repositorio.Setup(s => s.SetorExiste(builder.SetorId))
                .Returns(Task.FromResult(true));

            var setoresLista = new List<SetorAtendimentoDto>() 
            { 
                builder.Build() 
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.SetorExiste(builder.SetorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Once);

        }

        [Fact(DisplayName = "Vincular Setor ao Garcom. Comando Inválido Deve Falhar")]
        [Trait(nameof(VincularSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task VincularSetorGarcomCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new VincularSetorGarcomCommand(garcom.Id, builder.SetorId);
            garcom.VincularSetor(builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.SetorExiste(builder.SetorId))
                            .Returns(Task.FromResult(true));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            var setoresLista = new List<SetorAtendimentoDto>()
            {
                builder.Build()
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.SetorJaFoiVinculadoAoGarcom);
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.SetorExiste(builder.SetorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Never);
        }

        [Fact(DisplayName = "Vincular Setor ao Garcom. Banco Nao Salva Deve Falhar")]
        [Trait(nameof(VincularSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task VincularSetorGarcomCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new VincularSetorGarcomCommand(garcom.Id, builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            _fixture.Repositorio.Setup(s => s.SetorExiste(builder.SetorId))
                            .Returns(Task.FromResult(true));

            var setoresLista = new List<SetorAtendimentoDto>()
            {
                builder.Build()
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.SetorExiste(builder.SetorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Never);
        }

        [Fact(DisplayName = "Vincular Setor ao Garcom. Id Garcom Inválido Deve Falhar")]
        [Trait(nameof(VincularSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task VincularSetorGarcomCommand_IdGarcomInvalido_DeveFalhar()
        {
            // Arrange
            int garcomId = 1;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new VincularSetorGarcomCommand(garcomId, builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.SetorExiste(builder.SetorId))
                .Returns(Task.FromResult(true));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcomId, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(null));

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcomId))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(null));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, command.RaizAgregacaoId));
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcomId, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.SetorExiste(builder.SetorId), Times.Never);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Garcom>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcomId), Times.Never);
        }

        [Fact(DisplayName = "Vincular Setor ao Garcom. Setor Inexistente. Deve Falhar")]
        [Trait(nameof(VincularSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task VincularSetorGarcomCommand_SetorInexistente_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new VincularSetorGarcomCommand(garcom.Id, builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            _fixture.Repositorio.Setup(s => s.SetorExiste(builder.SetorId))
                .Returns(Task.FromResult(false));

            var setoresLista = new List<SetorAtendimentoDto>()
            {
                builder.Build()
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, builder.SetorId));
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.SetorExiste(builder.SetorId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Never);
        }


        [Fact(DisplayName = "Remover Setor Garcom. Comando Válido. Deve Ter Sucesso")]
        [Trait(nameof(RemoverSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task RemoverSetorGarcomCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            SetorAtendimentoDto setorRemover = builder.Build();
            SetorAtendimentoDto setorMantido = builder.ComId(2).ComSetorId(2).ComNomeSetor("Área Externa").Build();
            garcom.VincularSetor(setorRemover.SetorId);
            garcom.VincularSetor(setorMantido.SetorId);

            var command = new RemoverSetorGarcomCommand(garcom.Id, setorRemover.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            var setoresLista = new List<SetorAtendimentoDto>()
            {
                setorMantido
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Remover Setor Garcom. Comando Inválido Deve Falhar")]
        [Trait(nameof(RemoverSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task RemoverSetorGarcomCommand_ComandoInvalido_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            garcom.VincularSetor(builder.SetorId);
            int setorIdRemover = 2;
            var command = new RemoverSetorGarcomCommand(garcom.Id, setorIdRemover);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(null));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, command.SetorId));
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Never);
        }

        [Fact(DisplayName = "Remover Setor Garcom. Banco Nao Salva Deve Falhar")]
        [Trait(nameof(RemoverSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task RemoverSetorGarcomCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new RemoverSetorGarcomCommand(garcom.Id, builder.SetorId);
            int setorRemover = builder.SetorId;
            int setorManter = 2;
            garcom.VincularSetor(builder.SetorId);
            garcom.VincularSetor(setorManter);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcom.Id, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            var setoresLista = new List<SetorAtendimentoDto>()
            {
                builder.ComSetorId(setorManter).Build(),
                builder.ComSetorId(setorRemover).Build()
            };

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcom.Id))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(setoresLista));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcom.Id, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcom.Id), Times.Never);
        }

        [Fact(DisplayName = "Remover Setor Garcom. Id Garcom Inválido Deve Falhar")]
        [Trait(nameof(RemoverSetorGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task RemoverSetorGarcomCommand_IdGarcomInvalido_DeveFalhar()
        {
            // Arrange
            int garcomId = 1;
            var builder = new SetorAtendimentoDtoTestBuilder();
            var command = new RemoverSetorGarcomCommand(garcomId, builder.SetorId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterComSetoresAsync(garcomId, CancellationToken.None))
                .Returns(Task.FromResult<Maybe<Garcom>>(null));

            _fixture.Repositorio.Setup(s => s.ObterListaSetoresAsync(garcomId))
                .Returns(Task.FromResult<IEnumerable<SetorAtendimentoDto>>(null));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, command.RaizAgregacaoId));
            _fixture.Repositorio.Verify(r => r.ObterComSetoresAsync(garcomId, It.IsAny<CancellationToken>()), Times.Once);
            _fixture.Repositorio.Verify(r => r.Atualizar(It.IsAny<Garcom>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fixture.Repositorio.Verify(r => r.ObterListaSetoresAsync(garcomId), Times.Never);
        }

        [Fact(DisplayName = "Excluir Garcom. Comando Válido. Deve Ter Sucesso")]
        [Trait(nameof(ExcluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task ExcluirGarcomCommand_ComandoValido_DeveTerSucesso()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var command = new ExcluirGarcomCommand(garcom.Id);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcom.Id))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(garcom.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Value.ShouldBe(command.RaizAgregacaoId);
        }

        [Fact(DisplayName = "Excluir Garcom. Id Inválido. Deve Falhar")]
        [Trait(nameof(ExcluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task ExcluirGarcomCommand_IdInvalido_DeveFalhar()
        {
            // Arrange
            int garcomId = 1;
            var command = new ExcluirGarcomCommand(garcomId);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcomId))
                .Returns(Task.FromResult<Maybe<Garcom>>(null));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, command.RaizAgregacaoId));
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(garcomId), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(It.IsAny<Garcom>()), Times.Never);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Excluir Garcom. Banco Nao Salva. Deve Falhar")]
        [Trait(nameof(ExcluirGarcomCommand), nameof(GarconsCommandHandler.Handle))]
        public async Task ExcluirGarcomCommand_BancoNaoSalva_DeveFalhar()
        {
            // Arrange
            Garcom garcom = new GarcomTestBuilder().Build().Value;
            var command = new ExcluirGarcomCommand(garcom.Id);
            _fixture.RepositorioReset()
                .UnitOfWork.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));

            _fixture.Repositorio.Setup(s => s.ObterPorIdAsync(garcom.Id))
                .Returns(Task.FromResult<Maybe<Garcom>>(garcom));

            // Act
            Result<int> result = await _fixture.CommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NaoFoiPossivelExcluirGarcom);
            _fixture.Repositorio.Verify(r => r.ObterPorIdAsync(garcom.Id), Times.Once);
            _fixture.Repositorio.Verify(r => r.Remover(garcom), Times.Once);
            _fixture.UnitOfWork.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

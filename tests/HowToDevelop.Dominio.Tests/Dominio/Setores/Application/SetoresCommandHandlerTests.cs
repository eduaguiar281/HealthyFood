using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Application.Setores;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using HowToDevelop.HealthFood.Setores;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Setores.Application
{
    [Collection(nameof(SetoresCommandHandlerFixtureCollection))]
    public class SetoresCommandHandlerTests
    {
        private readonly SetoresCommandHandlerFixture _fixture;
        public SetoresCommandHandlerTests(SetoresCommandHandlerFixture fixture)
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
    }
}

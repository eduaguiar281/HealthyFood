using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using HowToDevelop.HealthFood.Setores;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using HowToDevelop.HealthFood.Setores.Application.Queries;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Setores.Application
{
    [Collection(nameof(SetoresHandlersFixtureCollection))]
    public class SetoresQueryHandlerTests
    {
        private readonly SetoresHandlersFixture _fixture;
        public SetoresQueryHandlerTests(SetoresHandlersFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Obter Setor Por Id Query Válida Deve Ter Sucesso")]
        [Trait(nameof(ObterSetorPorIdQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterSetorPorIdQuery_QueryValida_DeveTerSucesso()
        {
            // Arrange
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);
            setor.AdicionarMesa(3);
            var query = new ObterSetorPorIdQuery(setor.Id);
            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterComMesasPorIdAsync(setor.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(setor));

            // Act
            Result<SetorDto> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(setor.Id), Times.Once);
            result.Value.Mesas.Any(m => m.Numeracao == 1).ShouldBeTrue();
            result.Value.Mesas.Any(m => m.Numeracao == 2).ShouldBeTrue();
            result.Value.Mesas.Any(m => m.Numeracao == 3).ShouldBeTrue();
        }

        [Fact(DisplayName = "Obter Setor Por Id, SetorId inválido. Deve Falhar")]
        [Trait(nameof(ObterSetorPorIdQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterSetorPorIdQuery_SetorIdInvalido_DeveFalhar()
        {
            // Arrange
            var query = new ObterSetorPorIdQuery(0);

            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterComMesasPorIdAsync(query.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<SetorDto> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.IdSetorInformadoNaoEhValido);
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(query.Id), Times.Never);
        }

        [Fact(DisplayName = "Obter Setor Por Id, SetorId não localizado. Deve Falhar")]
        [Trait(nameof(ObterSetorPorIdQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterSetorPorIdQuery_SetorIdNaoLocalizado_DeveFalhar()
        {
            // Arrange
            var query = new ObterSetorPorIdQuery(1);
            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterComMesasPorIdAsync(query.Id))
                .Returns(Task.FromResult<Maybe<Setor>>(null));

            // Act
            Result<SetorDto> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, query.Id));
            _fixture.Repositorio.Verify(r => r.ObterComMesasPorIdAsync(query.Id), Times.Once);
        }


        [Fact(DisplayName = "Obter todos os Setores, Query Válida. Deve Ter Sucesso")]
        [Trait(nameof(ObterTodosSetoresQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterTodosSetoresQuery_QueryValida_DeveTerSucesso()
        {
            // Arrange
            List<SetorInfoDto> setores = new List<SetorInfoDto>
            {
                new SetorInfoDto {
                    Id = 1,
                    Nome = "Setor Vip",
                    Sigla = "VP1",
                    QuantidadeMesas = 5 ,
                    PossuiAtendente = true
                },
                new SetorInfoDto {
                    Id = 1,
                    Nome = "Setor Externo",
                    Sigla = "EX1",
                    QuantidadeMesas = 15,
                    PossuiAtendente = false
                },
                new SetorInfoDto {
                    Id = 1,
                    Nome = "Setor Interno",
                    Sigla = "IN1",
                    QuantidadeMesas = 35,
                    PossuiAtendente = true
                }
            };
            var query = new ObterTodosSetoresQuery();
            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterTodosSetorInfoAsync())
                .Returns(Task.FromResult<IEnumerable<SetorInfoDto>>(setores));

            // Act
            Result<IEnumerable<SetorInfoDto>> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _fixture.Repositorio.Verify(r => r.ObterTodosSetorInfoAsync(), Times.Once);
            result.Value.Count().ShouldBe(3);
        }

        [Fact(DisplayName = "Obter todos os Setores, Repositorio Retorna Null. Deve Falhar")]
        [Trait(nameof(ObterTodosSetoresQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterTodosSetoresQuery_RepositorioRetornaNull_DeveFalhar()
        {
            // Arrange
            var query = new ObterTodosSetoresQuery();
            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterTodosSetorInfoAsync())
                .Returns(Task.FromResult<IEnumerable<SetorInfoDto>>(null));

            // Act
            Result<IEnumerable<SetorInfoDto>> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NenhumSetorFoiEncontrado);
            _fixture.Repositorio.Verify(r => r.ObterTodosSetorInfoAsync(), Times.Once);
        }

        [Fact(DisplayName = "Obter todos os Setores, Repositorio Retorna Lista Vazia. Deve Falhar")]
        [Trait(nameof(ObterTodosSetoresQuery), nameof(SetoresQueryHandler.Handle))]
        public async Task ObterTodosSetoresQuery_RepositorioRetornaListaVaza_DeveFalhar()
        {
            // Arrange
            List<SetorInfoDto> setores = new List<SetorInfoDto>();
            var query = new ObterTodosSetoresQuery();
            _fixture.RepositorioReset()
                .Repositorio.Setup(s => s.ObterTodosSetorInfoAsync())
                .Returns(Task.FromResult<IEnumerable<SetorInfoDto>>(setores));

            // Act
            Result<IEnumerable<SetorInfoDto>> result = await _fixture.QueryHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NenhumSetorFoiEncontrado);
            _fixture.Repositorio.Verify(r => r.ObterTodosSetorInfoAsync(), Times.Once);
        }
    }
}

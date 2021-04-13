using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Dominio.Integration.Tests.Fixtures;
using HowToDevelop.HealthFood.Garcons.Application.Commands;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HowToDevelop.Dominio.Integration.Tests
{
    public class GarconsCommandHandlerTests : IAssemblyFixture<GarcomHandlerTestFixture>, IAssemblyFixture<HealthFoodDbContextTestFixture>
    {
        private readonly GarcomHandlerTestFixture _garcomFixture;
        private readonly HealthFoodDbContextTestFixture _fixture;

        public GarconsCommandHandlerTests(GarcomHandlerTestFixture garcomFixture, HealthFoodDbContextTestFixture fixture)
        {
            _garcomFixture = garcomFixture;
            _fixture = fixture;
        }

        [Fact(DisplayName = "Incluir Deve Ter Sucesso"), Order(1)]
        [Trait(nameof(GarconsCommandHandler), nameof(IncluirGarcomCommand))]
        public async Task GarconsCommandHandler_HandlerIncluirGarcomCommand_DeveTerSucesso()
        {
            // Arrange
            var command = new IncluirGarcomCommand(GarcomHandlerTestFixture.NomeGarcomIncluido,
                GarcomHandlerTestFixture.ApelidoGarcomIncluido);
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new GarconsCommandHandler(new GarconsRepositorio(context));

            //Act
            Result<GarcomDto> result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            _garcomFixture.GarcomIdIncluido = result.Value.Id;
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Apelido.ShouldBe(command.Apelido);
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Garcom garcomAssertion = await contextAssertion.Garcons
                .FirstOrDefaultAsync(s => s.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldNotBeNull();
            garcomAssertion.Nome.ShouldBe((Nome.Criar(GarcomHandlerTestFixture.NomeGarcomIncluido).Value));
            garcomAssertion.Apelido.ShouldBe((Apelido.Criar(GarcomHandlerTestFixture.ApelidoGarcomIncluido).Value));
        }

        [Fact(DisplayName = "Alterar Dados Pessoais Deve Ter Sucesso"), Order(2)]
        [Trait(nameof(GarconsCommandHandler), nameof(AlterarDadosPessoaisGarcomCommand))]
        public async Task GarconsCommandHandler_HandlerAlterarDadosPessoaisGarcomCommand_DeveTerSucesso()
        {
            // Arrange
            var command = new AlterarDadosPessoaisGarcomCommand(_garcomFixture.GarcomIdIncluido,
                GarcomHandlerTestFixture.NomeGarcomAlterado,
                GarcomHandlerTestFixture.ApelidoGarcomAlterado);
            
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new GarconsCommandHandler(new GarconsRepositorio(context));

            //Act
            Result<GarcomDto> result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.Nome.ShouldBe(command.Nome);
            result.Value.Apelido.ShouldBe(command.Apelido);
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Garcom garcomAssertion = await contextAssertion.Garcons
                .FirstOrDefaultAsync(s => s.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldNotBeNull();
            garcomAssertion.Nome.ShouldBe((Nome.Criar(GarcomHandlerTestFixture.NomeGarcomAlterado).Value));
            garcomAssertion.Apelido.ShouldBe((Apelido.Criar(GarcomHandlerTestFixture.ApelidoGarcomAlterado).Value));
        }

        [Fact(DisplayName = "Vincular Setor Ao Garcom Deve Ter Sucesso"), Order(3)]
        [Trait(nameof(GarconsCommandHandler), nameof(VincularSetorGarcomCommand))]
        public async Task GarconsCommandHandler_HandlerVincularSetorGarcomCommand_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            Setor setor = await context.Setores.FirstOrDefaultAsync();
            var command = new VincularSetorGarcomCommand(_garcomFixture.GarcomIdIncluido,
                setor.Id);
            _garcomFixture.SetorVinculado = setor.Id;
            var handler = new GarconsCommandHandler(new GarconsRepositorio(context));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await handler.Handle(command, CancellationToken.None);

            // Arrange
            result.IsSuccess.ShouldBeTrue();
            result.Value.Count().ShouldBe(1);
            result.Value.Any(s => s.SetorId == setor.Id).ShouldBeTrue();
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Garcom garcomAssertion = await contextAssertion.Garcons
                .Include(x => x.SetoresAtendimento)
                .FirstOrDefaultAsync(s => s.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldNotBeNull();
            garcomAssertion.SetoresAtendimento.Count.ShouldBe(1);
            garcomAssertion.SetoresAtendimento.Any(s => s.SetorId == setor.Id).ShouldBeTrue();
        }

        [Fact(DisplayName = "Remover Setor do Garcom Deve Ter Sucesso"), Order(4)]
        [Trait(nameof(GarconsCommandHandler), nameof(RemoverSetorGarcomCommand))]
        public async Task GarconsCommandHandler_HandlerRemoverSetorGarcomCommand_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            int ultimoSetorId = await context.Setores.MaxAsync(x => x.Id);
            Setor setorExcluir = await context.Setores.FindAsync(ultimoSetorId);
            Garcom garcom = await context.Garcons.Include(x=> x.SetoresAtendimento)
                .FirstOrDefaultAsync(x=> x.Id == _garcomFixture.GarcomIdIncluido);
            garcom.VincularSetor(setorExcluir.Id);
            await context.SaveChangesAsync();

            int totalSetores = garcom.SetoresAtendimento.Count;

            var command = new RemoverSetorGarcomCommand(_garcomFixture.GarcomIdIncluido,
                setorExcluir.Id);
            var handler = new GarconsCommandHandler(new GarconsRepositorio(context));

            // Act
            Result<IEnumerable<SetorAtendimentoDto>> result = await handler.Handle(command, CancellationToken.None);

            // Arrange
            result.IsSuccess.ShouldBeTrue();
            result.Value.Count().ShouldBe(totalSetores -1);
            result.Value.Any(s => s.SetorId == setorExcluir.Id).ShouldBeFalse();
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Garcom garcomAssertion = await contextAssertion.Garcons
                .Include(x=> x.SetoresAtendimento)
                .FirstOrDefaultAsync(s => s.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldNotBeNull();
            garcomAssertion.SetoresAtendimento.Count.ShouldBe(totalSetores - 1);
            garcomAssertion.SetoresAtendimento.Any(s => s.SetorId == setorExcluir.Id).ShouldBeFalse();
        }

        [Fact(DisplayName = "Remover Setor do Garcom Deve Ter Sucesso"), Order(5)]
        [Trait(nameof(GarconsCommandHandler), nameof(ExcluirGarcomCommand))]
        public async Task GarconsCommandHandler_HandlerExcluirGarcomCommand_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();

            var command = new ExcluirGarcomCommand(_garcomFixture.GarcomIdIncluido);
            var handler = new GarconsCommandHandler(new GarconsRepositorio(context));

            // Act
            Result<int> result = await handler.Handle(command, CancellationToken.None);

            // Arrange
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(_garcomFixture.GarcomIdIncluido);
            await context.DisposeAsync();

            using HealthFoodDbContext contextAssertion = _fixture.GetContext();
            Garcom garcomAssertion = await contextAssertion.Garcons
                .FirstOrDefaultAsync(s => s.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldBeNull();
        }
    }
}

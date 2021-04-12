using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Dominio.Integration.Tests.Fixtures;
using HowToDevelop.HealthFood.Garcons.Application.Commands;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using Microsoft.EntityFrameworkCore;
using Shouldly;
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

    }
}

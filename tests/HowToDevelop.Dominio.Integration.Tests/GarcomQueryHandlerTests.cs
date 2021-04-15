using CSharpFunctionalExtensions;
using HowToDevelop.Dominio.Integration.Tests.Fixtures;
using HowToDevelop.HealthFood.Garcons;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Garcons.Application.Queries;
using HowToDevelop.HealthFood.Garcons.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Setores;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HowToDevelop.Dominio.Integration.Tests
{
    public class GarcomQueryHandlerTests : IAssemblyFixture<GarcomTestFixture>, IAssemblyFixture<HealthFoodDbContextTestFixture>
    {
        private readonly GarcomTestFixture _garcomFixture;
        private readonly HealthFoodDbContextTestFixture _fixture;
        private int _quantidadeSetoresVinculado;
        public GarcomQueryHandlerTests(GarcomTestFixture garcomTestFixture, HealthFoodDbContextTestFixture fixture)
        {
            _garcomFixture = garcomTestFixture;
            _fixture = fixture;
            Inicializar();
        }

        
        private void Inicializar()
        {
            using HealthFoodDbContext context = _fixture.GetContext();
            var garcom = Garcom.Criar(GarcomTestFixture.NomeGarcomIncluido, GarcomTestFixture.NomeGarcomIncluido).Value;
            Setor setor = context.Setores.FirstOrDefault();
            garcom.VincularSetor(setor.Id);
            context.Garcons.Add(garcom);
            context.SaveChanges();
            _quantidadeSetoresVinculado = garcom.SetoresAtendimento.Count;
            _garcomFixture.GarcomIdIncluido = garcom.Id;
            _garcomFixture.SetorVinculado = setor.Id;
            _garcomFixture.NomeSetorVinculado = setor.Nome;
        }

        [Fact(DisplayName = "Obter Garcom Por Id Deve Ter Sucesso"), Order(1)]
        [Trait(nameof(GarconsQueryHandler), nameof(ObterGarcomPorIdQuery))]
        public async Task GarconsQueryHandler_HandlerObterGarcomPorIdQuery_DeveTerSucesso()
        {
            // Arrange
            var query = new ObterGarcomPorIdQuery(_garcomFixture.GarcomIdIncluido);
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new GarconsQueryHandler(new GarconsRepositorio(context));

            //Act
            Result<GarcomDto> result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.Nome.ShouldBe(GarcomTestFixture.NomeGarcomIncluido);
            result.Value.Apelido.ShouldBe(GarcomTestFixture.NomeGarcomIncluido);
            result.Value.Setores.Count().ShouldBe(_quantidadeSetoresVinculado);
            SetorAtendimentoDto setor = result.Value.Setores.FirstOrDefault(x => x.SetorId == _garcomFixture.SetorVinculado.Value);
            setor.ShouldNotBeNull();
            setor.NomeSetor.ShouldBe(_garcomFixture.NomeSetorVinculado);
        }

        [Fact(DisplayName = "Obter Garcom Por Id Deve Ter Sucesso"), Order(1)]
        [Trait(nameof(GarconsQueryHandler), nameof(ObterTodosGarconsQuery))]
        public async Task GarconsQueryHandler_HandlerObterTodosGarconsQuery_DeveTerSucesso()
        {
            // Arrange
            var query = new ObterTodosGarconsQuery();
            using HealthFoodDbContext context = _fixture.GetContext();
            var handler = new GarconsQueryHandler(new GarconsRepositorio(context));

            //Act
            Result<IEnumerable<GarcomInfoDto>> result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.Count().ShouldBeGreaterThan(1);
            GarcomInfoDto garcomAssertion = result.Value.FirstOrDefault(x => x.Id == _garcomFixture.GarcomIdIncluido);
            garcomAssertion.ShouldNotBeNull();
            garcomAssertion.QuantidadeSetores.ShouldBe(_quantidadeSetoresVinculado);
        }

    }
}

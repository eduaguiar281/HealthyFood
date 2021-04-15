using HowToDevelop.Dominio.Integration.Tests.Fixtures;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;
using Shouldly;
using HowToDevelop.Core.ObjetosDeValor;
using System.Linq;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using HowToDevelop.HealthFood.Setores.Infraestrutura;

namespace HowToDevelop.Dominio.Integration.Tests
{
    public class SetoresRepositorioTests : IAssemblyFixture<SetoresRepositorioTestFixture>, IAssemblyFixture<HealthFoodDbContextTestFixture>
    {
        private readonly SetoresRepositorioTestFixture _setoresFixture;
        private readonly HealthFoodDbContextTestFixture _fixture;
        public SetoresRepositorioTests(HealthFoodDbContextTestFixture fixture, 
            SetoresRepositorioTestFixture setoresFixture)
        {
            _setoresFixture = setoresFixture;
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Deve Ter Sucesso"), Order(1)]
        [Trait(nameof(SetoresRepositorio), nameof(SetoresRepositorio.Adicionar))]
        public async Task SetoresRepositorio_Adicionar_DeveTerSucesso()
        {
            // Arrange
            var setor = Setor.Criar(SetoresRepositorioTestFixture.NomeSetorIncluido, SetoresRepositorioTestFixture.SiglaSetorIncluido).Value;
            foreach (ushort mesa in _setoresFixture.MesasAdicionar)
                setor.AdicionarMesa(mesa);
            using HealthFoodDbContext context = _fixture.GetContext();
            var repositorio = new SetoresRepositorio(context);

            //Act
            repositorio.Adicionar(setor);
            await repositorio.UnitOfWork.CommitAsync();

            //Assert
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Setor setorAssertion = await contextAssertion.Setores.Include(s => s.Mesas)
                .FirstOrDefaultAsync(s => s.Id == setor.Id);
            setorAssertion.ShouldNotBeNull();
            setorAssertion.Nome.ShouldBe((Nome.Criar(SetoresRepositorioTestFixture.NomeSetorIncluido).Value));
            setorAssertion.Sigla.ShouldBe((Sigla.Criar(SetoresRepositorioTestFixture.SiglaSetorIncluido).Value));
            setorAssertion.Mesas.Count.ShouldBe(_setoresFixture.MesasAdicionar.Length);
            setorAssertion.Mesas.ToList()
                .ForEach(x => _setoresFixture.MesasAdicionar.ShouldContain(x.Numeracao));
            _setoresFixture.SetorIdIncluido = setorAssertion.Id;
        }

        [Fact(DisplayName = "Atualizar Deve Ter Sucesso"), Order(2)]
        [Trait(nameof(SetoresRepositorio), nameof(SetoresRepositorio.Atualizar))]
        public async Task SetoresRepositorio_Atualizar_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            var repositorio = new SetoresRepositorio(context);
            Setor setorAlterar = await context.Setores.Include(s => s.Mesas)
                .FirstOrDefaultAsync(s => s.Id == _setoresFixture.SetorIdIncluido);
            setorAlterar.AlterarDescricaoSetor(SetoresRepositorioTestFixture.NomeSetorAlterado, 
                SetoresRepositorioTestFixture.SiglaSetorAlterado);
            setorAlterar.RemoverMesa(SetoresRepositorioTestFixture.MesaRemover);


            //Act
            repositorio.Atualizar(setorAlterar);
            await repositorio.UnitOfWork.CommitAsync();

            //Assert
            await context.DisposeAsync();
            using HealthFoodDbContext contextAssertion = _fixture.GetContext();

            Setor setorAssertion = await contextAssertion.Setores.Include(s => s.Mesas)
                .FirstOrDefaultAsync(s => s.Id == setorAlterar.Id);
            setorAssertion.ShouldNotBeNull();
            setorAssertion.Nome.ShouldBe((Nome.Criar(SetoresRepositorioTestFixture.NomeSetorAlterado).Value));
            setorAssertion.Sigla.ShouldBe((Sigla.Criar(SetoresRepositorioTestFixture.SiglaSetorAlterado).Value));
            Mesa mesaRemovida = setorAssertion.Mesas.FirstOrDefault(m => m.Numeracao == SetoresRepositorioTestFixture.MesaRemover);
            mesaRemovida.ShouldBeNull();
            setorAssertion.Mesas.Count.ShouldBe(_setoresFixture.MesasAdicionar.Length -1);
        }

        [Fact(DisplayName = "ObterPorIdAsync Deve Ter Sucesso"), Order(3)]
        [Trait(nameof(SetoresRepositorio), nameof(SetoresRepositorio.ObterPorIdAsync))]
        public async Task SetoresRepositorio_ObterPorIdAsync_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            var repositorio = new SetoresRepositorio(context);

            //Act
            Maybe<Setor> setor =  await repositorio.ObterPorIdAsync(_setoresFixture.SetorIdIncluido);

            //Assert
            setor.HasValue.ShouldBeTrue();
            var setorAssertion = setor.Value;
            setorAssertion.Nome.ShouldBe((Nome.Criar(SetoresRepositorioTestFixture.NomeSetorAlterado).Value));
            setorAssertion.Sigla.ShouldBe((Sigla.Criar(SetoresRepositorioTestFixture.SiglaSetorAlterado).Value));
            setorAssertion.Mesas.Count.ShouldBe(0);
        }

        [Fact(DisplayName = "ObterComMesasPorIdAsync Deve Ter Sucesso"), Order(4)]
        [Trait(nameof(SetoresRepositorio), nameof(SetoresRepositorio.ObterComMesasPorIdAsync))]
        public async Task SetoresRepositorio_ObterComMesasPorIdAsync_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            var repositorio = new SetoresRepositorio(context);

            //Act
            Maybe<Setor> setor = await repositorio.ObterComMesasPorIdAsync(_setoresFixture.SetorIdIncluido);

            //Assert
            setor.HasValue.ShouldBeTrue();
            var setorAssertion = setor.Value;
            setorAssertion.Nome.ShouldBe((Nome.Criar(SetoresRepositorioTestFixture.NomeSetorAlterado).Value));
            setorAssertion.Sigla.ShouldBe((Sigla.Criar(SetoresRepositorioTestFixture.SiglaSetorAlterado).Value));
            setorAssertion.Mesas.Count.ShouldBe(_setoresFixture.MesasAdicionar.Length -1);
            setorAssertion.Mesas.FirstOrDefault(m => m.Numeracao == SetoresRepositorioTestFixture.MesaRemover)
                .ShouldBeNull();
        }
        
        [Fact(DisplayName = "ObterTodosSetorInfoAsync Deve Ter Sucesso"), Order(5)]
        [Trait(nameof(SetoresRepositorio), nameof(SetoresRepositorio.ObterTodosSetorInfoAsync))]
        public async Task SetoresRepositorio_ObterTodosSetorInfoAsync_DeveTerSucesso()
        {
            // Arrange
            using HealthFoodDbContext context = _fixture.GetContext();
            var repositorio = new SetoresRepositorio(context);

            //Act
            IEnumerable<SetorInfoDto> setores = await repositorio.ObterTodosSetorInfoAsync();

            //Assert
            setores.Count().ShouldBe(4);
            setores.Any(s => s.PossuiAtendente).ShouldBeTrue();
            SetorInfoDto setorAssertion = setores.FirstOrDefault(x => x.Id == _setoresFixture.SetorIdIncluido);
            setorAssertion.ShouldNotBeNull();
            setorAssertion.QuantidadeMesas.ShouldBe(_setoresFixture.MesasAdicionar.Length - 1);
            setorAssertion.PossuiAtendente.ShouldBeFalse();

        }
    }
}

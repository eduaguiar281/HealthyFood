using AutoMapper;
using Bogus;
using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.Core.StoredEvents;
using HowToDevelop.DockerUtils.Artifacts;
using HowToDevelop.HealthFood.Garcons;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Setores;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using HowToDevelop.EventSourcing;
using static Bogus.DataSets.Name;

namespace HowToDevelop.Dominio.Integration.Tests.Helpers
{
    public class HealthFoodDbContextTestHelper
    {
        private readonly string _databaseConnectionString;
        private readonly MongoDbDockerSettings _mongoDbDockerSettings;
        private const int MaximoSetores = 3;
        private const int MaximoGarcons = 2;

        public HealthFoodDbContextTestHelper(string databaseConnectionString, MongoDbDockerSettings mongoDbDockerSettings)
        {
            _databaseConnectionString = databaseConnectionString;
            _mongoDbDockerSettings = mongoDbDockerSettings;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DomainModelToDtoMappingProfile()));
            AutoMapperConfiguration.Init(config);
        }

        public HealthFoodDbContext CreateContext()
        {
            var eventStoreService = new EventStoreService(new EventStoreRepository(_mongoDbDockerSettings.GetConnectionString(), 
                _mongoDbDockerSettings.DatabaseName));
            var mediator = new Mock<IMediator>();
            var mediatorHandler = new MediatorHandler(mediator.Object, eventStoreService);
            return new HealthFoodDbContext(new DbContextOptionsBuilder<HealthFoodDbContext>().UseSqlServer(_databaseConnectionString).Options, mediatorHandler);
        }

        public string GetConnectionString() => _databaseConnectionString;

        public async Task InitializeDatabase()
        {
            await SeedDatabase();
        }

        private async Task SeedDatabase()
        {
            using var context = CreateContext();
            context.Database.Migrate();
            await SeedData(context);
        }

        private async Task SeedData(HealthFoodDbContext context)
        {
            await PopularSetores(context);
            await PopularGarcons(context);
        }

        private async Task PopularGarcons(HealthFoodDbContext context)
        {
            var faker = new Faker("pt_BR");
            for (int i = 1; i <= MaximoGarcons; i++)
            {
                var gen = faker.PickRandom<Gender>();
                var garcom = Garcom.Criar($"{faker.Name.FirstName(gen)} {faker.Name.LastName(gen)}", $"Garçom {i:D2}").Value;
                garcom.VincularSetor(context.Setores.FirstOrDefault().Id);
                context.Garcons.Add(garcom);
            }
            await context.SaveChangesAsync();
        }

        private async Task PopularSetores(HealthFoodDbContext context)
        {
            for (int i = 1; i <= MaximoSetores; i++)
            {
                var setor = Setor.Criar($"SETOR {i:D2}", $"S{i:D2}").Value;
                setor.AdicionarMesa((ushort)(i + 1));
                setor.AdicionarMesa((ushort)(i + 2));
                setor.AdicionarMesa((ushort)(i + 3));
                context.Setores.Add(setor);
            }
            await context.SaveChangesAsync();
        }

        public async Task CleanupTestsAndDropDatabaseAsync()
        {
            using var context = CreateContext();
            await context.Database.EnsureDeletedAsync();
        }
    }
}

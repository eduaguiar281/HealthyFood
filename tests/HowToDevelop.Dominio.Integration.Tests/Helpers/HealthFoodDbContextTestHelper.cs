using Bogus;
using HowToDevelop.Core.Comunicacao;
using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using static Bogus.DataSets.Name;

namespace HowToDevelop.Dominio.Integration.Tests.Helpers
{
    public class HealthFoodDbContextTestHelper
    {
        private readonly string _databaseConnectionString;
        private readonly Mock<IMediatorHandler> _mediatorHandler;
        public const int _maximoSetores = 3;
        public const int _maximoGarcons = 2;

        public HealthFoodDbContextTestHelper(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
            _mediatorHandler = new Mock<IMediatorHandler>();
            _mediatorHandler.Setup(m => m.PublicarEvento(It.IsAny<Evento>()))
                .Returns(Task.CompletedTask);
        }

        public HealthFoodDbContext CreateContext()
        {
            return new HealthFoodDbContext(new DbContextOptionsBuilder<HealthFoodDbContext>().UseSqlServer(_databaseConnectionString).Options, _mediatorHandler.Object);
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
            for (int i = 1; i <= _maximoGarcons; i++)
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
            for (int i = 1; i <= _maximoSetores; i++)
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

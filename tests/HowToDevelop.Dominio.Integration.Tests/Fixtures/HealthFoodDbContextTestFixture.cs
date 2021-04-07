using Docker.DotNet;
using HowToDevelop.DockerUtils.Artifacts;
using HowToDevelop.Dominio.Integration.Tests.Helpers;
using HowToDevelop.HealthFood.Infraestrutura;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HowToDevelop.Dominio.Integration.Tests.Fixtures
{
    /// <summary>
    /// Fixture para testes de Integração.
    /// O IAssemblyFixture determina que o escopo da classe de teste será executado uma vez por projeto de teste. 
    /// Ou seja, InitializeAsync e DisposeAsync esta a nível de Suite de testes.
    ///     -> Assembly Scope <AssemblyFixture>.
    ///         -> Class Scope <ClassFixture>.
    ///             -> Test Scope <Constructor> and <Dispose>.
    /// </summary>

    public class HealthFoodDbContextTestFixture : IAsyncLifetime
    {

        private readonly IDockerClient _dockerClient = DockerClientBuilder.Build();
        public const string DATABASE_NAME_PLACEHOLDER = "ApplicationTestDatabase";
        private readonly DockerRegistries _dockerRegistries;

        protected HealthFoodDbContextTestHelper _dbContextTestHelper;
        private string _connectionString;
        private readonly SqlServerDockerSettings _settings;

        public HealthFoodDbContextTestFixture()
        {
            _dockerRegistries = new DockerRegistries();

            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            _settings = configuration.GetSection("SqlServerDockerSettings").Get<SqlServerDockerSettings>()
                ?? SqlServerDockerSettings.Default;

            _dockerRegistries.RegisterSqlServer2019(_dockerClient, _settings);
        }
        public async Task DisposeAsync()
        {
            await _dbContextTestHelper.CleanupTestsAndDropDatabaseAsync();
            await _dockerRegistries.CleanAsync();
        }

        public async Task InitializeAsync()
        {
            await _dockerRegistries.RunAsync();
            _connectionString = _settings.GetDatabaseConnectionString();
            _dbContextTestHelper = new HealthFoodDbContextTestHelper(_connectionString);
            await _dbContextTestHelper.InitializeDatabase();
        }

        public HealthFoodDbContext GetContext() => _dbContextTestHelper.CreateContext();
        protected string ConnectionString => _connectionString;
    }
}

using HowToDevelop.Dominio.Tests.Fixtures;
using HowToDevelop.HealthFood.Garcons.Application.Commands;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Garcons.Application
{
    [CollectionDefinition(nameof(GarconsHandlersFixtureCollection))]
    public class GarconsHandlersFixtureCollection : ICollectionFixture<GarconsHandlersFixture> { }

    public class GarconsHandlersFixture : FixtureCommandHandlerBase<IGarconsRepositorio>
    {
        private readonly GarconsCommandHandler _commandHandler;

        public GarconsHandlersFixture()
            : base()
        {
            _commandHandler = new GarconsCommandHandler(_repositorio.Object);
        }

        public GarconsCommandHandler CommandHandler => _commandHandler;
    }
}

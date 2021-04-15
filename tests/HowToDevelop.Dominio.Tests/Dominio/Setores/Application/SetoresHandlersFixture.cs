using HowToDevelop.Dominio.Tests.Fixtures;
using HowToDevelop.HealthFood.Setores.Application.Commands;
using HowToDevelop.HealthFood.Setores.Application.Queries;
using HowToDevelop.HealthFood.Setores.Infraestrutura;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Setores.Application
{
    [CollectionDefinition(nameof(SetoresHandlersFixtureCollection))]
    public class SetoresHandlersFixtureCollection : ICollectionFixture<SetoresHandlersFixture> { }

    public class SetoresHandlersFixture : FixtureCommandHandlerBase<ISetoresRepositorio>
    {
        private readonly SetoresCommandHandler _commandHandler;
        private readonly SetoresQueryHandler _queryHandler;

        public SetoresHandlersFixture()
        {
            _commandHandler = new SetoresCommandHandler(_repositorio.Object);
            _queryHandler = new SetoresQueryHandler(_repositorio.Object);

        }

        public SetoresCommandHandler CommandHandler => _commandHandler;
        public SetoresQueryHandler QueryHandler => _queryHandler;
    }
}

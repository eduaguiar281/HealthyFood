using HowToDevelop.Dominio.Tests.Fixtures;
using HowToDevelop.HealthFood.Produtos.Application.Commands;
using HowToDevelop.HealthFood.Produtos.Infraestrutura;
using Xunit;

namespace HowToDevelop.Dominio.Tests.Dominio.Produtos.Application
{
    [CollectionDefinition(nameof(ProdutosHandlersFixtureCollection))]
    public class ProdutosHandlersFixtureCollection : ICollectionFixture<ProdutosHandlersFixture> { }

    public class ProdutosHandlersFixture : FixtureCommandHandlerBase<IProdutosRepositorio>
    {
        private readonly ProdutosCommandHandler _commandHandler;

        public ProdutosHandlersFixture()
            : base()
        {
            _commandHandler = new ProdutosCommandHandler(_repositorio.Object);
        }

        public ProdutosCommandHandler CommandHandler => _commandHandler;


    }
}

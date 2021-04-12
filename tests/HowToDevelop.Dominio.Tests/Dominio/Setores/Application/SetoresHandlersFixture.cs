using AutoMapper;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Setores.Application.Commands;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using Moq;
using Moq.Language.Flow;
using System;
using System.Linq.Expressions;
using Xunit;
using HowToDevelop.HealthFood.Setores.Application.Queries;
using HowToDevelop.Dominio.Tests.Fixtures;

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

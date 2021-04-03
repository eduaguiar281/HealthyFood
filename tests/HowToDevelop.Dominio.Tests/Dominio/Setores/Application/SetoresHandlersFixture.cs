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

namespace HowToDevelop.Dominio.Tests.Dominio.Setores.Application
{
    [CollectionDefinition(nameof(SetoresHandlersFixtureCollection))]
    public class SetoresHandlersFixtureCollection : ICollectionFixture<SetoresHandlersFixture> { }

    public class SetoresHandlersFixture
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<ISetoresRepositorio> _repositorio;
        private readonly SetoresCommandHandler _commandHandler;
        private readonly SetoresQueryHandler _queryHandler;

        public SetoresHandlersFixture()
        {
            _repositorio = new Mock<ISetoresRepositorio>();
            _uow = new Mock<IUnitOfWork>();
            _repositorio.SetupGet(r => r.UnitOfWork).Returns(_uow.Object);
            _commandHandler = new SetoresCommandHandler(_repositorio.Object);
            _queryHandler = new SetoresQueryHandler(_repositorio.Object);

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DomainModelToDtoMappingProfile()));
            AutoMapperConfiguration.Init(config);
        }

        public Mock<ISetoresRepositorio> Repositorio => _repositorio;
        public Mock<IUnitOfWork> UnitOfWork => _uow;
        public SetoresCommandHandler CommandHandler => _commandHandler;
        public SetoresQueryHandler QueryHandler => _queryHandler;

        public SetoresHandlersFixture RepositorioReset()
        {
            _uow.Reset();
            _repositorio.Reset();
            _repositorio.SetupGet(r => r.UnitOfWork).Returns(_uow.Object);
            return this;
        }
        public ISetup<ISetoresRepositorio, TResult> RepositorioSetup<TResult>(Expression<Func<ISetoresRepositorio, TResult>> expression)
        {
            return _repositorio.Setup(expression);
        }

    }
}

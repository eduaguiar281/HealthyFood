using AutoMapper;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using Moq;
using Moq.Language.Flow;
using System;
using System.Linq.Expressions;

namespace HowToDevelop.Dominio.Tests.Fixtures
{
    public abstract class FixtureCommandHandlerBase<T> where T : class, IRepository
    {
        protected readonly Mock<IUnitOfWork> _uow;
        protected readonly Mock<T> _repositorio;

        protected FixtureCommandHandlerBase()
        {
            _repositorio = new Mock<T>();
            _uow = new Mock<IUnitOfWork>();
            _repositorio.SetupGet(r => r.UnitOfWork).Returns(_uow.Object);

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DomainModelToDtoMappingProfile()));
            AutoMapperConfiguration.Init(config);
        }

        public Mock<T> Repositorio => _repositorio;
        public Mock<IUnitOfWork> UnitOfWork => _uow;


        public FixtureCommandHandlerBase<T> RepositorioReset()
        {
            _uow.Reset();
            _repositorio.Reset();
            _repositorio.SetupGet(r => r.UnitOfWork).Returns(_uow.Object);
            return this;
        }

        public ISetup<T, TResult> RepositorioSetup<TResult>(Expression<Func<T, TResult>> expression)
        {
            return _repositorio.Setup(expression);
        }

    }
}

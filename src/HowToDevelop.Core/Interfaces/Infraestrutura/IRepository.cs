using HowToDevelop.Core.Entidade;

namespace HowToDevelop.Core.Interfaces.Infraestrutura
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<in T> : IRepository where T : RaizAgregacao
    {
        void Adicionar(T data);
        void Remover(T data);
        void Atualizar(T data);
    }
}

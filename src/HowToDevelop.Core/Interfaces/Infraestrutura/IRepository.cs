using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Interfaces.Infraestrutura
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<in T> : IRepository where T : IRaizAgregacao
    {
        void Adicionar(T data);
        void Remover(T data);
        void Atualizar(T data);
    }
}

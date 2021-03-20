using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Interfaces.Infraestrutura
{
    public interface IRepository<in T> where T : IRaizAgregacao
    {
        IUnitOfWork UnitOfWork { get; }
        void Adicionar(T data);
        void Remover(T data);
        void Atualizar(T data);
    }
}

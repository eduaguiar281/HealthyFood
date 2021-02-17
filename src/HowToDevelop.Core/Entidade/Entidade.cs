using CSharpFunctionalExtensions;
using System;


namespace HowToDevelop.Core
{
    public abstract class Entidade<T>: Entity<T> where T:struct
    {
        protected Entidade()
        {
        }

        protected Entidade(T id)
        {
            Id = id;
        }

        public virtual Result EhValido()
        {
            throw new NotImplementedException();
        }
    }
}

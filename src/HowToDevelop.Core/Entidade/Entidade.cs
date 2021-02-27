using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao;
using System;
using System.Collections.Generic;

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

        public DateTime DataCriacao { get; protected set; }
        public DateTime DataAlteracao { get; protected set; }

    }
}

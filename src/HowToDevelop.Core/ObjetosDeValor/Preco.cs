using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Preco : ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Preco()
        {
        }

        public Preco(decimal valor)
        {
            if (valor < 0)
            {
                throw new ArgumentException(PrecoConstantes.PrecoNaoPodeSerInferiorZero, nameof(valor));
            }

            Valor = valor;
        }

        public decimal Valor { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public static implicit operator decimal(Preco d) => d.Valor;
        public static explicit operator Preco(decimal valor) => new Preco(valor);

    }
}

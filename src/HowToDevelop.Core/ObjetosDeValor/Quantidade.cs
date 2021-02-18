using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Quantidade: ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Quantidade()
        {

        }

        public Quantidade(int valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentException(QuantidadeConstantes.QuantidadeNaoPodeSerInferiorOuIgualZero, nameof(valor));
            }
            Valor = valor;
        }

        public int Valor { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public static implicit operator int(Quantidade d) => d.Valor;
        public static explicit operator Quantidade(int valor) => new Quantidade(valor);

    }
}

using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Total: ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Total()
        {
        }

        public Total(decimal valor)
        {
            if (valor < 0)
            {
                throw new ArgumentException(TotalConstantes.TotalNaoPodeSerInferiorZero, nameof(valor));
            }

            Valor = valor;
        }

        public decimal Valor { get; private set; }

        [ExcludeFromCodeCoverage]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public static implicit operator decimal(Total d) => d.Valor;
        public static explicit operator Total(decimal valor) => new Total(valor);
    }
}

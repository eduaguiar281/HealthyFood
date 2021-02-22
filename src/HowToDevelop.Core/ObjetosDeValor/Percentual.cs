using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Percentual: ValueObject
    {
        [ExcludeFromCodeCoverage]
        private Percentual()
        {

        }

        private Percentual(in decimal valor)
        {
            Valor = valor / 100;
            ValorNominal = $"{valor:N1} %";
        }

        public decimal Valor { get; }
        public string ValorNominal { get; }

        [ExcludeFromCodeCoverage]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public static implicit operator decimal(Percentual d) => d.Valor;

        public static Result<Percentual> Criar(in decimal valor)
        {
            var (_, isFailure, error) = Result.Combine(valor.DeveSerMaiorQue(-1, PercentualConstantes.PercentualNaoDeveSerMenorQueZero),
                valor.DeveSerMenorOuIgualQue(100, PercentualConstantes.PercentualNaoDeveSerMaiorQueCem));

            if (isFailure)
            {
                return Result.Failure<Percentual>(error);
            }

            return Result.Success(new Percentual(valor));
        }
    }
}

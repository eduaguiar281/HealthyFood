using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor
{
    public class Comissao : ValueObject
    {
        [ExcludeFromCodeCoverage]
        private Comissao()
        {

        }

        private Comissao(in decimal baseCalculo, in Percentual percentual, in decimal gorjeta)
        {
            BaseCalculo = baseCalculo;
            Percentual = percentual;
            Gorjeta = gorjeta;
            Valor = BaseCalculo * Percentual;
            Total = new Total(Valor + Gorjeta);
        }

        public decimal Valor { get; }
        public decimal BaseCalculo { get; }
        public Percentual Percentual { get; }
        public decimal Gorjeta { get; }
        public Total Total { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
            yield return BaseCalculo;
            yield return Percentual;
            yield return Gorjeta;
            yield return Total;
        }

        public static implicit operator decimal(Comissao d) => d.Valor;

        public static Result<Comissao> Criar(in decimal baseCalculo, in decimal percentual, in decimal gorjeta)
        {
            var result = Percentual.Criar(percentual);
            if (result.IsFailure)
            {
                return Result.Failure<Comissao>(result.Error);
            }

            var (_, isFailure, error) = baseCalculo.DeveSerMaiorQueZero(ComissaoConstantes.BaseCalculoNaoPodeSerIgualZero);

            if (isFailure)
            {
                return Result.Failure<Comissao>(error);
            }

            return Result.Success(new Comissao(baseCalculo, result.Value, gorjeta));
        }

    }
}

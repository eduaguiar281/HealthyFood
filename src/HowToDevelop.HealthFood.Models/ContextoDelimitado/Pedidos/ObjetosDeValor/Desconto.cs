using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor
{
    public enum TipoDesconto { Percentual, Valor };

    public class Desconto : ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Desconto () { }

        private Desconto(in decimal valor, in Percentual percentual, in decimal baseCalculo, in TipoDesconto tipo)
        {
            Valor = valor;
            BaseCalculo = baseCalculo;
            Percentual = percentual;
            TipoDescontoPedido = tipo;
        }

        public Percentual Percentual { get; private set; }
        public decimal Valor { get; private set; }
        public decimal BaseCalculo { get; private set; }
        public TipoDesconto TipoDescontoPedido { get; private set; }

        public static implicit operator decimal(Desconto d) => d.Valor;

        public static Result<Desconto> CriarPorValor(in decimal valor, in decimal baseCalculo)
        {
            var (_, isFailure, error) = Result.Combine(baseCalculo.DeveSerMaiorQue(0, DescontoConstantes.BaseDeCaluculoNaoPodeSerIgualZero),
                valor.DeveSerMaiorQue(-1, DescontoConstantes.ValorDescontoNaoPodeSerMenorQueZero));

            if (isFailure)
            {
                return Result.Failure<Desconto>(error);
            }

            Percentual percentual = Percentual.Criar((valor/baseCalculo)*100).Value;

            return Result.Success(new Desconto(valor, percentual, baseCalculo, TipoDesconto.Valor));
        }

        public static Result<Desconto> CriarPorPercentual(in decimal percentual, in decimal baseCalculo)
        {
            var percentualResult = Percentual.Criar(percentual);
            if (percentualResult.IsFailure)
            {
                return Result.Failure<Desconto>(percentualResult.Error);
            }

            var (_, isFailure, error) = Result.Combine(
                baseCalculo.DeveSerMaiorQue(0, DescontoConstantes.BaseDeCaluculoNaoPodeSerIgualZero),
                percentual.DeveSerMaiorQue(0, DescontoConstantes.PercentualDeveSerMaiorQueZero));

            if (isFailure)
            {
                return Result.Failure<Desconto>(error);
            }

            decimal valor = (baseCalculo * percentual) / 100;

            return Result.Success(new Desconto(valor, percentualResult.Value, baseCalculo, TipoDesconto.Percentual));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Percentual;
            yield return Valor;
            yield return BaseCalculo;
            yield return TipoDescontoPedido;
        }
    }
}

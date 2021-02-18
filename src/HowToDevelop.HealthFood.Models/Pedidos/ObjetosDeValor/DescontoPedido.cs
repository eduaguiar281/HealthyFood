using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System;

namespace HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor
{
    public enum TipoDescontoPedido { Percentual, Valor };

    public class DescontoPedido
    {
        private DescontoPedido(in decimal valor, in decimal baseCalculo)
        {
            Valor = valor;
            BaseCalculo = baseCalculo;
            TipoDescontoPedido = TipoDescontoPedido.Valor;
        }

        private DescontoPedido(in decimal valor, in decimal percentual, in decimal baseCalculo)
        {
            Valor = valor;
            BaseCalculo = baseCalculo;
            Percentual = percentual;
            TipoDescontoPedido = TipoDescontoPedido.Percentual;
        }

        public decimal Percentual { get; private set; }
        public decimal Valor { get; private set; }
        public decimal BaseCalculo { get; private set; }
        public TipoDescontoPedido TipoDescontoPedido { get; private set; }


        public static Result<DescontoPedido> CriarPorValor(in decimal valor, in decimal baseCalculo)
        {
            var (_, isFailure, error) = valor.DeveSerMaiorQue(-1, DescontoPedidoConstantes.ValorDescontoNaoPodeSerMenorQueZero);

            if (isFailure)
            {
                return Result.Failure<DescontoPedido>(error);
            }

            return Result.Success(new DescontoPedido(valor, baseCalculo));
        }

        public static Result<DescontoPedido> CriarPorPercentual(in decimal percentual, in decimal baseCalculo)
        {
            var (_, isFailure, error) = Result.Combine(
                percentual.DeveSerMaiorQue(0, DescontoPedidoConstantes.PercentualDeveSerMaiorQueZero),
                percentual.DeveSerMenorOuIgualQue(DescontoPedidoConstantes.PercentualMaximo, 
                                                  DescontoPedidoConstantes.PercentualDescontoNaoDeveUltrapassarPercentualMaximo));

            if (isFailure)
            {
                return Result.Failure<DescontoPedido>(error);
            }

            decimal valor = (baseCalculo * percentual) / 100;

            return Result.Success(new DescontoPedido(valor, percentual, baseCalculo));
        }
    }
}

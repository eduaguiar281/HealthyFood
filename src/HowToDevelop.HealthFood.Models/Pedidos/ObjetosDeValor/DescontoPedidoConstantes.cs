using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor
{
    public static class DescontoPedidoConstantes
    {
        public const string ValorDescontoNaoPodeSerMenorQueZero = "Valor de desconto não pode ser menor que zero!";

        public const string PercentualDeveSerMaiorQueZero = "Percentual de desconto deve ser maior que zero!";

        public static readonly string PercentualDescontoNaoDeveUltrapassarPercentualMaximo = $"Percentual não pode ser superior a {PercentualMaximo:0.0}%!";

        public const decimal PercentualMaximo = 50m;
    }
}

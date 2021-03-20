using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Infraestrutura.Pedidos
{
    public static class PedidosConstantes
    {
        public const string ItensPedidoProdutoIdNaoEhValido = "ProdutoId não é válido!";
        public const string ItensPedidoQuantidadeEhObrigatorio = "Quantidade é Obrigatório!";
        public const string ItensPedidoPrecoEhObrigatorio = "Preço é Obrigatório!";
        public const string PedidosItemInformadoNaoFoiLocalizado = "Item informado {0} não foi localizado!";

        public const string PedidosGarcomIdNaoEhValido = "Id do Garçom não é Válido!";
        public const string PedidosMesaIdNaoEhValido = "Id da Mesa não é Válido!";
        public const string PedidosNomeClienteEhObrigatorio = "Nome do Cliente é Obrigatório!";

        public const string PedidoFechadoNaoPodeSerAlterado = "Pedido Fechado não pode ser alterado!";
        public const string PedidoCanceladoNaoPodeSerAlterado = "Pedido Cancelado não pode ser alterado!";

        public const int PedidosTamanhoNomeCliente = 70;
        public static readonly string PedidosNomeClienteDeveTerNoMaxmimoNCaracteres = $"Nome do Cliente deve ter no máximo {PedidosTamanhoNomeCliente} caracteres!";

        public static readonly string PercentualDescontoNaoDeveUltrapassarPercentualMaximo = $"Percentual não pode ser superior a {PercentualMaximo:0.0}%!";
        public const decimal PercentualMaximo = 0.3m;

        public const string PedidoDeveEstarEmAndamento = "Para fazer o fechamento o pedido deve estar em andamento!";
        public const string PedidoJaFoiCancelado = "Pedido já foi cancelado!";
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Pedidos
{
    public sealed class ItensPedido: Entidade<int>
    {
        private ItensPedido(in int produtoId, in int quantidade, in decimal preco, int id = 0)
            :base(id)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Preco = preco;
            CalcularTotal();
        }
        public int ProdutoId { get; }
        public int Quantidade { get; private set; }
        public decimal Preco { get; private set; }

        private decimal _totalItem;
        public decimal TotalItem => _totalItem;

        public Result AlterarValores(in int quantidade, in decimal preco)
        {
            var (_, isFailure, erro) = ValidarPrecoQuantidade(quantidade, preco);

            if (isFailure)
                return Result.Failure(erro);

            Quantidade = quantidade;
            Preco = preco;

            CalcularTotal();

            return Result.Success();
        }

        private void CalcularTotal()
        {
            _totalItem = Quantidade * Preco;
        }

        private Result ValidarPrecoQuantidade(in int quantidade, in decimal preco)
        {
            return Result.Combine(quantidade.DeveSerMaiorQue(0, PedidosConstantes.ItensPedidoQuantidadeDeveSerMaiorQueZero),
                preco.DeveSerMaiorQue(0, PedidosConstantes.ItensPedidoPrecoDeveSerMaiorQueZero));
        }

        public override Result EhValido()
        {
            return Result.Combine(
                ProdutoId.DeveSerMaiorQue(0, PedidosConstantes.ItensPedidoProdutoIdNaoEhValido),
                ValidarPrecoQuantidade(Quantidade, Preco));
        }

        public static Result<ItensPedido> Criar(in int produtoId, in int quantidade, in decimal preco, int id = 0)
        {
            var item = new ItensPedido(produtoId, quantidade, preco, id);

            var (_, isFailure, error) = item.EhValido();
            if (isFailure)
                return Result.Failure<ItensPedido>(error);

            return Result.Success(item);
        }
    }
}

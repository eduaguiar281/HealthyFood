using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System;

namespace HowToDevelop.HealthFood.Dominio.Pedidos
{
    public sealed class ItensPedido: Entidade<int>
    {
        private ItensPedido() { }
        private ItensPedido(in int produtoId, in Quantidade quantidade, in Preco preco, int id)
            :base(id)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Preco = preco;
            CalcularTotal();
        }
        public int ProdutoId { get; }
        public Quantidade Quantidade { get; private set; }
        public Preco Preco { get; private set; }

        private Total _totalItem;
        public Total TotalItem => _totalItem;

        public Result AlterarValores(in Quantidade quantidade, in Preco preco)
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
            _totalItem = new Total(Quantidade * Preco);
        }

        private static Result ValidarPrecoQuantidade(in Quantidade quantidade, in Preco preco)
        {
            return Result.Combine(quantidade.NaoDeveSerNulo(PedidosConstantes.ItensPedidoQuantidadeEhObrigatorio),
                preco.NaoDeveSerNulo(PedidosConstantes.ItensPedidoPrecoEhObrigatorio));
        }

        private static Result ValidarDadosDoItem(in int produtoId, in Quantidade quantidade, in Preco preco)
        {
            return Result.Combine(
                produtoId.DeveSerMaiorQue(0, PedidosConstantes.ItensPedidoProdutoIdNaoEhValido),
                ValidarPrecoQuantidade(quantidade, preco));
        }

        public static Result<ItensPedido> Criar(in int produtoId, in Quantidade quantidade, in Preco preco, int id = 0)
        {
            var (_, isFailure, error) = Result.Combine(produtoId.DeveSerMaiorQue(0, PedidosConstantes.ItensPedidoProdutoIdNaoEhValido),
                    ValidarDadosDoItem(produtoId, quantidade, preco));

            if (isFailure)
                return Result.Failure<ItensPedido>(error);

            return Result.Success(new ItensPedido(produtoId, quantidade, preco, id));
        }
    }
}

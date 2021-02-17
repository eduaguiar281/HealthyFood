using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using HowToDevelop.HealthFood.Dominio.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Pedidos
{
    public sealed class Pedido: Entidade<int>
    {

        private Pedido()
        {
            _itens = new List<ItensPedido>();
        }
        public Pedido(int mesaId, int garcomId, string nomeCliente)
        {
            MesaId = mesaId;
            GarcomId = garcomId;
            NomeCliente = nomeCliente;
            _total = 0;
            _desconto = 0;
            _itens = new List<ItensPedido>();
        }

        public Pedido(int id, int mesaId, int garcomId, string nomeCliente)
            :base(id)
        {
            MesaId = mesaId;
            GarcomId = garcomId;
            NomeCliente = nomeCliente;
            _total = 0;
            _desconto = 0;
            _itens = new List<ItensPedido>();
        }

        public int MesaId { get; }

        public int GarcomId { get; }

        public string NomeCliente { get; private set; }

        private decimal _total;
        public decimal Total => _total;

        private decimal _desconto;
        public decimal Desconto => _desconto;

        private List<ItensPedido> _itens;

        public IReadOnlyCollection<ItensPedido> Itens => _itens.AsReadOnly();

        public Result AdicionarItem(in int produtoId, in int quantidade, decimal preco, int id = 0)
        {
            var (_, isFailure, item, error) = ItensPedido.Criar(produtoId, quantidade, preco, id);
            
            if (isFailure)
                return Result.Failure<ItensPedido>(error);

            _itens.Add(item);
            CalcularTotal();

            return Result.Success();
        }
        public Result AlterarItem(int itemId, in int quantidade, decimal preco)
        {
            Maybe<ItensPedido> item = _itens.FirstOrDefault(i => i.Id == itemId);
            
            if (item.HasNoValue)
                return Result.Failure(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, itemId));
            
            var (_, isFailure, erro) = item.Value.AlterarValores(quantidade, preco);

            if (isFailure)
                return Result.Failure(erro);

            CalcularTotal();

            return Result.Success();
        }

        public Result RemoverItem(int itemId)
        {
            Maybe<ItensPedido> item = _itens.FirstOrDefault(i => i.Id == itemId);

            if (item.HasNoValue)
                return Result.Failure(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, itemId));

            _itens.Remove(item.Value);

            CalcularTotal();

            return Result.Success();
        }

        private void CalcularTotal()
        {
            _total = _itens.Sum(t => t.TotalItem);
        }

        public override Result EhValido()
        {
            return Result.Combine(
                GarcomId.DeveSerMaiorQue(0, PedidosConstantes.PedidosGarcomIdNaoEhValido),
                MesaId.DeveSerMaiorQue(0, PedidosConstantes.PedidosMesaIdNaoEhValido),
                NomeCliente.NaoDeveSerNuloOuVazio(PedidosConstantes.PedidosNomeClienteEhObrigatorio),
                NomeCliente.TamanhoMenorOuIgual(PedidosConstantes.PedidosTamanhoNomeCliente, 
                                                PedidosConstantes.PedidosNomeClienteDeveTerNoMaxmimoNCaracteres));
        }
    }
}

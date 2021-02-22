﻿using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.Interfaces;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HowToDevelop.HealthFood.Dominio.Pedidos
{
    public enum StatusPedido { Novo, EmAndamento, Fechado, Cancelado }

    public sealed class Pedido: Entidade<int>, IRaizAgregacao
    {
        [ExcludeFromCodeCoverage]
        private Pedido()
        {
            _itens = new List<ItensPedido>();
        }

        private Pedido(int id, in int mesaId, in int garcomId, in string nomeCliente)
            :base(id)
        {
            MesaId = mesaId;
            GarcomId = garcomId;
            NomeCliente = nomeCliente;
            _itens = new List<ItensPedido>();
            CalcularTotal();
            Status = StatusPedido.Novo;
        }

        public int MesaId { get; }

        public int GarcomId { get; }

        public string NomeCliente { get; private set; }

        public StatusPedido Status { get; private set; }

        private Total _total;
        public Total Total => _total;

        private Desconto _desconto;
        public Desconto Desconto => _desconto;

        private readonly List<ItensPedido> _itens;

        public IReadOnlyCollection<ItensPedido> Itens => _itens.AsReadOnly();

        public Result AdicionarItem(in int produtoId, in Quantidade quantidade, Preco preco, int id = 0)
        {
            Result result = PedidoPodeSerAlterado();
            if (result.IsFailure)
            {
                return result;
            }

            var (_, isFailure, item, error) = ItensPedido.Criar(produtoId, quantidade, preco, id);

            if (isFailure)
            {
                return Result.Failure<ItensPedido>(error);
            }

            _itens.Add(item);

            CalcularTotal();

            AlteraStatusParaAndamento();

            return Result.Success();
        }

        public Result AlterarItem(int itemId, in Quantidade quantidade, Preco preco)
        {
            Result result = PedidoPodeSerAlterado();
            if (result.IsFailure)
            {
                return result;
            }
            Maybe<ItensPedido> item = _itens.FirstOrDefault(i => i.Id == itemId);

            if (item.HasNoValue)
            {
                return Result.Failure(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, itemId));
            }
            
            var (_, isFailure, erro) = item.Value.AlterarValores(quantidade, preco);

            if (isFailure)
            {
                return Result.Failure(erro);
            }

            CalcularTotal();

            return Result.Success();
        }

        public Result RemoverItem(int itemId)
        {
            Result result = PedidoPodeSerAlterado();
            if (result.IsFailure)
            {
                return result;
            }
            Maybe<ItensPedido> item = _itens.FirstOrDefault(i => i.Id == itemId);

            if (item.HasNoValue)
            {
                return Result.Failure(string.Format(PedidosConstantes.PedidosItemInformadoNaoFoiLocalizado, itemId));
            }

            _itens.Remove(item.Value);

            CalcularTotal();
            
            return Result.Success();
        }

        private void CalcularTotal()
        {
            _total = (Total)_itens.Sum(t => t.TotalItem.Valor);
        }

        private Result PedidoPodeSerAlterado()
        {
            if (Status == StatusPedido.Fechado)
            {
                return Result.Failure(PedidosConstantes.PedidoFechadoNaoPodeSerAlterado);
            }

            if (Status == StatusPedido.Cancelado)
            {
                return Result.Failure(PedidosConstantes.PedidoCanceladoNaoPodeSerAlterado);
            }

            return Result.Success();
        }

        private void AlteraStatusParaAndamento()
        {
            Status = StatusPedido.EmAndamento;
        }

        private static Result ValidarDadosPedido(in int garcomId, in int mesaId, in string nomeCliente)
        {
            return Result.Combine(
                garcomId.DeveSerMaiorQue(0, PedidosConstantes.PedidosGarcomIdNaoEhValido),
                mesaId.DeveSerMaiorQue(0, PedidosConstantes.PedidosMesaIdNaoEhValido),
                nomeCliente.NaoDeveSerNuloOuVazio(PedidosConstantes.PedidosNomeClienteEhObrigatorio),
                nomeCliente.TamanhoMenorOuIgual(PedidosConstantes.PedidosTamanhoNomeCliente, 
                                                PedidosConstantes.PedidosNomeClienteDeveTerNoMaxmimoNCaracteres));
        }

        public static Result<Pedido> Criar(in int mesaId, in int garcomId, in string nomeCliente, int id = 0)
        {
            var (_, isFailure, error) = ValidarDadosPedido(garcomId, mesaId, nomeCliente);
            if (isFailure)
            {
                return Result.Failure<Pedido>(error);
            }

            return Result.Success(new Pedido(id, mesaId, garcomId, nomeCliente));
        }

    }
}

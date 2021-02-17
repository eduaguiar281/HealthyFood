using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class ItensPedidoTestBuilder: ITestBuilder<Result<ItensPedido>>
    {
        public ItensPedidoTestBuilder()
        {
            Reiniciar();               
        }
        public int Id { get; private set; }
        public int ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Preco { get; private set; }

        public ItensPedidoTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }
        public ItensPedidoTestBuilder ComProdutoId(int produtoId)
        {
            ProdutoId = produtoId;
            return this;
        }
        public ItensPedidoTestBuilder ComQuantidade(int quantidade)
        {
            Quantidade = quantidade;
            return this;
        }
        public ItensPedidoTestBuilder ComPreco(decimal preco)
        {
            Preco = preco;
            return this;
        }


        public Result<ItensPedido> Build()
        {
            return ItensPedido.Criar(ProdutoId, Quantidade, Preco, Id);
        }

        public void Reiniciar()
        {
            Id = 1;
            ProdutoId = 1;
            Quantidade = 10;
            Preco = 5.5m;
        }
    }
}

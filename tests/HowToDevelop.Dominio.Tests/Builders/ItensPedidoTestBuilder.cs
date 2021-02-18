using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
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
        public Quantidade Quantidade { get; private set; }
        public Preco Preco { get; private set; }

        public ItensPedidoTestBuilder ComQuantidade(Quantidade quantidade)
        {
            Quantidade = quantidade;
            return this;
        }
        public ItensPedidoTestBuilder ComPreco(Preco preco)
        {
            Preco = preco;
            return this;
        }


        public Result<ItensPedido> Build()
        {
            return ItensPedido.Criar(1, Quantidade, Preco, 1);
        }

        public void Reiniciar()
        {
            Quantidade = new Quantidade(10);
            Preco = new Preco(5.5m);
        }
    }
}

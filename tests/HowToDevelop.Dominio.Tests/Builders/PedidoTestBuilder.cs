using HowToDevelop.HealthFood.Dominio.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class PedidoTestBuilder : ITestBuilder<Pedido>
    {
        public PedidoTestBuilder()
        {
            Reiniciar();        
        }

        public int Id { get; private set; }
        public PedidoTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }

        public int MesaId { get; private set; }
        public PedidoTestBuilder ComMesaId(int mesaId)
        {
            MesaId = mesaId;
            return this;
        }

        public int GarcomId { get; private set; }
        public PedidoTestBuilder ComGarcomId(int garcomId)
        {
            GarcomId = garcomId;
            return this;
        }


        public string NomeCliente { get; private set; }
        public PedidoTestBuilder ComNomeCliente(string nome)
        {
            NomeCliente = nome;
            return this;
        }

        public IEnumerable<ItensPedido> Itens { get; private set; }
        public PedidoTestBuilder ComItens(IEnumerable<ItensPedido> itens)
        {
            Itens = itens;
            return this;
        }


        public Pedido Build()
        {
            var pedido = new Pedido(Id, MesaId, GarcomId, NomeCliente);
            
            foreach (var item in Itens)
                pedido.AdicionarItem(item.ProdutoId, item.Quantidade, item.Preco, item.Id);
            
            return pedido;
        }

        public void Reiniciar()
        {
            Id = 1;
            NomeCliente = "José da Silva";
            MesaId = 1;
            GarcomId = 1;
            Itens = new List<ItensPedido>()
            {
                ItensPedido.Criar(1, 2, 1.99m, 1).Value,
                ItensPedido.Criar(2, 5, 3.45m, 2).Value,
                ItensPedido.Criar(2, 3, 2.07m, 3).Value
            };
        }
    }
}

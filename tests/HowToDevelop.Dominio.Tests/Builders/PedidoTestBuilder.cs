using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class PedidoTestBuilder : ITestBuilder<Result<Pedido>>
    {
        public PedidoTestBuilder()
        {
            Reiniciar();        
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

        public Result<Pedido> Build()
        {
            return Pedido.Criar(MesaId, GarcomId, NomeCliente);
        }

        public void Reiniciar()
        {
            NomeCliente = "José da Silva";
            MesaId = 1;
            GarcomId = 1;
        }
    }
}

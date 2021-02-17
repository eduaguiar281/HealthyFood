using HowToDevelop.HealthFood.Dominio.Produtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class ProdutoTestBuilder : ITestBuilder<Produto>
    {

        public ProdutoTestBuilder()
        {
            Reiniciar();
        }
        
        public int Id { get; private set; }
        public string CodigoBarras { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public TipoProduto TipoProduto { get; private set; }

        public ProdutoTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }
        public ProdutoTestBuilder ComCodigoBarras(string codigoBarras)
        {
            CodigoBarras = codigoBarras;
            return this;
        }

        public ProdutoTestBuilder ComDescricao(string descricao)
        {
            Descricao = descricao;
            return this;
        }

        public ProdutoTestBuilder ComPreco(decimal preco)
        {
            Preco = preco;
            return this;
        }
        
        public ProdutoTestBuilder ComTipoProduto(TipoProduto tipo)
        {
            TipoProduto = tipo;
            return this;
        }

        public Produto Build()
        {
            return new Produto(Id)
            {
                CodigoBarras = this.CodigoBarras,
                Descricao = this.Descricao,
                Preco = this.Preco,
                TipoProduto = this.TipoProduto
            };
        }

        public void Reiniciar()
        {
            Id = 1;
            CodigoBarras = "7891213030969";
            Descricao = "Coca-Cola";
            Preco = 3.99m;
            TipoProduto = TipoProduto.Bebida;
        }
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Produtos;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Builders
{
    public class ProdutoTestBuilder : ITestBuilder<Result<Produto>>
    {

        public ProdutoTestBuilder()
        {
            Reiniciar();
        }

        public string CodigoBarras { get; private set; }
        public string Descricao { get; private set; }
        public Preco Preco { get; private set; }
        public TipoProduto Tipo { get; private set; }

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

        public ProdutoTestBuilder ComPreco(Preco preco)
        {
            Preco = preco;
            return this;
        }

        public ProdutoTestBuilder ComTipo(TipoProduto tipo)
        {
            Tipo = tipo;
            return this;
        }

        public Result<Produto> Build()
        {
            switch (Tipo)
            {
                case TipoProduto.Bebida:
                    {
                        return Produto.CriarTipoBebida(CodigoBarras, Descricao, Preco, 1);
                    }
                case TipoProduto.Lanche:
                    {
                        return Produto.CriarTipoLanche(CodigoBarras, Descricao, Preco, 1);
                    }
                case TipoProduto.Outros:
                    {
                        return Produto.CriarTipoOutros(CodigoBarras, Descricao, Preco, 1);
                    }
                default:
                    {
                        return Produto.CriarTipoOutros(CodigoBarras, Descricao, Preco, 1);
                    }
            }
        }

        public void Reiniciar()
        {
            CodigoBarras = "7891213030969";
            Descricao = "Coca-Cola";
            Preco = new Preco(3.99m);
            Tipo = TipoProduto.Bebida;
        }
    }
}

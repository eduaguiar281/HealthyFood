using HowToDevelop.HealthFood.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Dominio.Integration.Tests.Fixtures
{
    public class ProdutoTestFixture
    {
        public const string DescricaoProdutoIncluido = "Suco de Morango";
        public const string CodigoBarrasProdutoIncluido = "7891213030969";
        public const TipoProduto TipoProdutoIncluido = TipoProduto.Bebida;
        public const decimal PrecoProdutoIncluido = 5.99m;

        public const string DescricaoProdutoAlterado = "Suco de Morango com Leite";
        public const string CodigoBarrasProdutoAlterado = "7891213030968";
        public const decimal PrecoProdutoAlterado = 6.99m;

        public ProdutoTestFixture()
        {

        }
        public int ProdutoIdIncluido { get; set; }
    }
}

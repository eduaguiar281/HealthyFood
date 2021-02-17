using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;

namespace HowToDevelop.HealthFood.Dominio.Produtos
{
    public sealed class Produto: Entidade<int>
    {

        public Produto()
        {
            TipoProduto = TipoProduto.Outros;
        }

        public Produto(int id)
            :base(id)
        {
            TipoProduto = TipoProduto.Outros;
        }

        public string CodigoBarras { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public TipoProduto TipoProduto { get; set; }

        public override Result EhValido()
        {
            return Result.Combine(
                CodigoBarras.NaoDeveSerNuloOuVazio(ProdutosConstantes.ProdutoCodigoBarrasEhObrigatorio),
                CodigoBarras.TamanhoMenorOuIgual(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras, ProdutosConstantes.ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres),
                Descricao.NaoDeveSerNuloOuVazio(ProdutosConstantes.ProdutoDescricaoEhObrigatorio),
                Descricao.TamanhoMenorOuIgual(ProdutosConstantes.ProdutoTamanhoCampoDescricao, ProdutosConstantes.ProdutoDescricaoDeveTerNoMaximoNCaracteres),
                Preco.DeveSerMaiorQue(0, ProdutosConstantes.ProdutoPrecoNaoPodeSerMenorOuIgualZero));
        }
    }
}

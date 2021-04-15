using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;

namespace HowToDevelop.HealthFood.Produtos.Application.Commands
{
    public class IncluirProdutoCommand : Command<ProdutoDto>
    {
        public IncluirProdutoCommand(in string codigoBarras, in string descricao, in decimal preco, in TipoProduto tipoProduto)
        {
            CodigoBarras = codigoBarras;
            Descricao = descricao;
            Preco = preco;
            TipoProduto = tipoProduto;
        }
        public string CodigoBarras { get; }
        public string Descricao { get; }
        public decimal Preco { get; }
        public TipoProduto TipoProduto { get; }
    }
}

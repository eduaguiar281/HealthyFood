using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;

namespace HowToDevelop.HealthFood.Produtos.Application.Commands
{
    public class AlterarDadosProdutoCommand : Command<ProdutoDto, int>
    {
        public AlterarDadosProdutoCommand(in int id, in string codigoBarras, in string descricao, in decimal preco)
            : base(id)
        {
            CodigoBarras = codigoBarras;
            Descricao = descricao;
            Preco = preco;
        }

        public string CodigoBarras { get; }
        public string Descricao { get; }
        public decimal Preco { get; }
    }
}

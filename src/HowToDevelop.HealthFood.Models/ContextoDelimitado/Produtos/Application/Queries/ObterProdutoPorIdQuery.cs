using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;

namespace HowToDevelop.HealthFood.Produtos.Application.Queries
{
    public class ObterProdutoPorIdQuery : IQuery<ProdutoDto>
    {
        public ObterProdutoPorIdQuery(in int id)
        {
            Id = id;
        }
        public int Id { get; }
    }
}

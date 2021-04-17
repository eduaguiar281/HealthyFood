using HowToDevelop.Core.Interfaces.Infraestrutura;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Produtos.Infraestrutura
{
    public interface IProdutosRepositorio : IRepository<Produto>
    {
        Task<Produto> ObterPorIdAsync(int id);
    }
}

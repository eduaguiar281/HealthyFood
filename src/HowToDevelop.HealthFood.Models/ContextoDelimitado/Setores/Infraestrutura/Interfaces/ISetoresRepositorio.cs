using CSharpFunctionalExtensions;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Setores;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    public interface ISetoresRepositorio: IRepository<Setor>
    {
        Task<Maybe<Setor>> ObterPorIdAsync(int id);

        Task<Maybe<Setor>> ObterComMesasPorIdAsync(int id);
    }
}

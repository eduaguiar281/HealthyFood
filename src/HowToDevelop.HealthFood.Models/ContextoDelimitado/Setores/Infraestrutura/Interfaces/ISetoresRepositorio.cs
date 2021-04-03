using CSharpFunctionalExtensions;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Setores;
using System.Collections.Generic;
using System.Threading.Tasks;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    public interface ISetoresRepositorio: IRepository<Setor>
    {
        Task<Maybe<Setor>> ObterPorIdAsync(int id);

        Task<Maybe<Setor>> ObterComMesasPorIdAsync(int id);

        Task<IEnumerable<SetorInfoDto>> ObterTodosSetorInfoAsync();
    }
}

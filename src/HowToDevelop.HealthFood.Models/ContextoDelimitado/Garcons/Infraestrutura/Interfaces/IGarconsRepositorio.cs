using CSharpFunctionalExtensions;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Infraestrutura.Garcons
{
    public interface IGarconsRepositorio : IRepository<Garcom>
    {
        Task<Maybe<Garcom>> ObterPorIdAsync(int id);

        Task<Maybe<Garcom>> ObterComSetoresAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<SetorAtendimentoDto>> ObterListaSetoresAsync(int garcomId);

        Task<bool> SetorExiste(int setorId);

        Task<IEnumerable<GarcomInfoDto>> ObterTodosGarconsAsync()
    }
}

using HowToDevelop.Core.Comunicacao;
using HowToDevelop.Core.Comunicacao.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HowToDevelop.Core.StoredEvents
{
    public interface IEventStoreService
    {
        Task Salvar(IEventoDominio evento);
        Task<IEnumerable<StoredEvent>> ObterEventos(int aggregateId);
    }
}

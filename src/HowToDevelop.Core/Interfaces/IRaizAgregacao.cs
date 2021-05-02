using HowToDevelop.Core.Comunicacao.Interfaces;
using System.Collections.Generic;

namespace HowToDevelop.Core.Interfaces
{
    public interface IRaizAgregacao<TKey> where TKey : struct
    {
        TKey Id { get; }
        IReadOnlyCollection<IEventoDominio> Notificacoes { get; }

        void AdicionarEvento(IEventoDominio evento);

        void RemoverEvento(IEventoDominio eventItem);

        void LimparEventos();

    }
}

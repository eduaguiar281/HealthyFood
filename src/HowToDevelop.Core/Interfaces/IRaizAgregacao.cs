using HowToDevelop.Core.Comunicacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.Core.Interfaces
{
    public interface IRaizAgregacao
    {

        IReadOnlyCollection<Evento> Notificacoes { get; }

        void AdicionarEvento(Evento evento);

        void RemoverEvento(Evento eventItem);

        void LimparEventos();

    }
}

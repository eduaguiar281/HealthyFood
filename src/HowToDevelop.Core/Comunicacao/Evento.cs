using HowToDevelop.Core.Comunicacao.Interfaces;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Evento: Mensagem, IEvento
    {
        public DateTime Timestamp { get; private set; }

        protected Evento()
            :base()
        {
            Timestamp = DateTime.UtcNow;
        }

    }
}

using MediatR;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Evento: Mensagem, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Evento()
        {
            Timestamp = DateTime.UtcNow;
        }

    }
}

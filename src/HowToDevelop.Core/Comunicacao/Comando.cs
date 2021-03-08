using HowToDevelop.Core.Comunicacao.Interfaces;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Comando<T>:Mensagem, IComando<T>
    {
        protected Comando()
        {
            Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; protected set; }
    }
}

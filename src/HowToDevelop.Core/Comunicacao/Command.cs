using HowToDevelop.Core.Comunicacao.Interfaces;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Command<T>:Mensagem, ICommand<T>
    {
        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; protected set; }
    }
}

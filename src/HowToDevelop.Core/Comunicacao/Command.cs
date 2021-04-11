using HowToDevelop.Core.Comunicacao.Interfaces;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Command<T, TKey>:Mensagem, ICommand<T, TKey>
    {
        protected Command(in TKey raizAgregacaoId)
        {
            Timestamp = DateTime.UtcNow;
            RaizAgregacaoId = raizAgregacaoId;
        }

        public DateTime Timestamp { get; protected set; }

        public TKey RaizAgregacaoId { get; }
    }

    public abstract class Command<T> : Mensagem, ICommand<T>
    {
        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; protected set; }
    }
}

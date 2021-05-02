using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.Interfaces;
using System.Collections.Generic;

namespace HowToDevelop.Core.Entidade
{
    public abstract class RaizAgregacao<T> : Entidade<T>, IRaizAgregacao<T> where T: struct
    {
        protected RaizAgregacao()
        {
                
        }
        protected RaizAgregacao(T id)
            :base(id)
        {

        }

        private List<IEventoDominio> _notificacoes;
        public IReadOnlyCollection<IEventoDominio> Notificacoes => _notificacoes?.AsReadOnly();

        public void AdicionarEvento(IEventoDominio evento)
        {
            _notificacoes ??= new List<IEventoDominio>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(IEventoDominio eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

    }

    public abstract class RaizAgregacao : RaizAgregacao<int>
    {
        protected RaizAgregacao()
        {

        }
        protected RaizAgregacao(int id)
            : base(id)
        {

        }
    }
}

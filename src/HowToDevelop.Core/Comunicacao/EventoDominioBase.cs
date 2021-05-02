using HowToDevelop.Core.Comunicacao.Interfaces;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class EventoDominioBase<TKey> : Evento where TKey : struct
    {
        protected EventoDominioBase(in TKey raizAgregacaoId)
            :base()
        {
            RaizAgregacaoId = raizAgregacaoId;
        }
        public TKey RaizAgregacaoId { get; set; }
    }

    public abstract class EventoDominioBase : EventoDominioBase<int> , IEventoDominio
    {
        protected EventoDominioBase(in int raizAgregacaoId)
            : base(raizAgregacaoId)
        {

        }
    }

}

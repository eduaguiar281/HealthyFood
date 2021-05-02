namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface IEventoDominio<TKey> : IEvento where TKey : struct 
    {
        TKey RaizAgregacaoId { get; set; }

        object Data { get; }

        string MensagemTipo { get; }
    }
    public interface IEventoDominio : IEventoDominio<int>
    {
    }
}

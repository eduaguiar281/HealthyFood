namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Mensagem
    {
        public string MensagemTipo { get; protected set; }

        protected Mensagem()
        {
            MensagemTipo = GetType().Name;
        }

        public object Data { get; set; }

        public TResult ConverterPara<TResult>() where TResult : class
        {
            try
            {
                return (TResult)Data;
            }
            catch
            {
                return default;
            }

        }
    }
}

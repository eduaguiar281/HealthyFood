using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao.Interfaces;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Comunicacao.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Evento;

        Task<Result<T>> EnviarComando<T>(IComando<T> comando);  
    }
}

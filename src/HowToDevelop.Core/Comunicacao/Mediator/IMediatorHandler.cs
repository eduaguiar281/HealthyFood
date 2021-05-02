using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao.Interfaces;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Comunicacao.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : IEventoDominio;

        Task<Result<T>> EnviarComando<T>(ICommand<T> comando);

        Task<Result<T>> EnviarQuery<T>(IQuery<T> query);
    }
}

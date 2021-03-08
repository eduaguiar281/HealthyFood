using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Comunicacao.Mediator
{
    public class MediatorHandler: IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<T>> EnviarComando<T>(IComando<T> comando)
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Evento
        {
            await _mediator.Publish(evento);
        }
    }
}

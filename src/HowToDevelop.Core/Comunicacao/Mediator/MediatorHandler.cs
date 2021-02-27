using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task PublicarEvento<T>(T evento) where T : Evento
        {
            await _mediator.Publish(evento);
        }
    }
}

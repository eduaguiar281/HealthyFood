using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Comunicacao.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Evento;
    }
}

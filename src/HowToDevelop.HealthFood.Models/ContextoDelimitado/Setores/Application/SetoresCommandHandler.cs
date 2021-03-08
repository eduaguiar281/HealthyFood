using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Setores.Application
{
    public class SetoresCommandHandler
        : IRequestHandler<IncluirSetorCommand, Result<SetorDto>>
    {
        public async Task<Result<SetorDto>> Handle(IncluirSetorCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

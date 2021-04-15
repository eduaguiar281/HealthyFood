using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using HowToDevelop.HealthFood.Setores.Infraestrutura;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Setores.Application.Queries
{
    public class SetoresQueryHandler :
        IRequestHandler<ObterSetorPorIdQuery, Result<SetorDto>>,
        IRequestHandler<ObterTodosSetoresQuery, Result<IEnumerable<SetorInfoDto>>>
    {
        private readonly ISetoresRepositorio _repositorio;
        public SetoresQueryHandler(ISetoresRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<Result<SetorDto>> Handle(ObterSetorPorIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return Result.Failure<SetorDto>(SetoresConstantes.IdSetorInformadoNaoEhValido);
            }

            Maybe<Setor> setor = await _repositorio.ObterComMesasPorIdAsync(request.Id);
            if (setor.HasNoValue)
            {
                return Result.Failure<SetorDto>(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, request.Id));
            }
            return Result.Success(setor.Value.Map<SetorDto>());
        }

        public async Task<Result<IEnumerable<SetorInfoDto>>> Handle(ObterTodosSetoresQuery request, CancellationToken cancellationToken)
        {
            var setores = Maybe<IEnumerable<SetorInfoDto>>.From(await _repositorio.ObterTodosSetorInfoAsync());
            if ((setores.HasNoValue)||(!setores.Value.Any()))
            {
                return Result.Failure<IEnumerable<SetorInfoDto>>(SetoresConstantes.NenhumSetorFoiEncontrado);
            }
            return Result.Success(setores.Value);
        }
    }
}

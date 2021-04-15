using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Garcons.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Garcons.Application.Queries
{
    public class GarconsQueryHandler : IRequestHandler<ObterGarcomPorIdQuery, Result<GarcomDto>>,
        IRequestHandler<ObterTodosGarconsQuery, Result<IEnumerable<GarcomInfoDto>>>
    {
        private readonly IGarconsRepositorio _repositorio;

        public GarconsQueryHandler(IGarconsRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<GarcomDto>> Handle(ObterGarcomPorIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return Result.Failure<GarcomDto>(GarconsConstantes.GarcomIdNaoEhValido);
            }

            Maybe<Garcom> garcom = await _repositorio.ObterPorIdAsync(request.Id);
            if (garcom.HasNoValue)
            {
                return Result.Failure<GarcomDto>(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, request.Id));
            }
            
            var dto = garcom.Value.Map<GarcomDto>();
            dto.Setores = await _repositorio.ObterListaSetoresAsync(dto.Id);

            return Result.Success(dto);
        }

        public async Task<Result<IEnumerable<GarcomInfoDto>>> Handle(ObterTodosGarconsQuery request, CancellationToken cancellationToken)
        {
            var garcons = Maybe<IEnumerable<GarcomInfoDto>>.From(await _repositorio.ObterTodosGarconsAsync());
            if ((garcons.HasNoValue) || (!garcons.Value.Any()))
            {
                return Result.Failure<IEnumerable<GarcomInfoDto>>(GarconsConstantes.NenhumGarcomFoiEncontrado);
            }

            return Result.Success(garcons.Value);
        }
    }
}

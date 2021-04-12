using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class GarconsCommandHandler : IRequestHandler<IncluirGarcomCommand, Result<GarcomDto>>
    {
        private readonly IGarconsRepositorio _repositorio;
        public GarconsCommandHandler(IGarconsRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        
        public async Task<Result<GarcomDto>> Handle(IncluirGarcomCommand request, CancellationToken cancellationToken)
        {
            var (_, isFailure, garcom, error) = Garcom.Criar(request.Nome, request.Apelido);

            if (isFailure)
            {
                return Result.Failure<GarcomDto>(error);
            }

            _repositorio.Adicionar(garcom);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure<GarcomDto>(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            }

            return Result.Success(garcom.Map<GarcomDto>());
        }
    }
}

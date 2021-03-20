using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Setores;

namespace HowToDevelop.HealthFood.Application.Setores
{
    public class SetoresCommandHandler
        : IRequestHandler<IncluirSetorCommand, Result<SetorDto>>
    {
        private readonly ISetoresRepositorio _repositorio;
        public SetoresCommandHandler(ISetoresRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Result<SetorDto>> Handle(IncluirSetorCommand request, CancellationToken cancellationToken)
        {
            var (_, isFailure, setor, error) = Setor.Criar(request.Nome, request.Sigla);

            if (isFailure)
            {
                return Result.Failure<SetorDto>(error);
            }

            _repositorio.Adicionar(setor);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            
            if (result <= 0)
            {
                return Result.Failure<SetorDto>(SetoresConstantes.NaoFoiPossivelSalvarSetor);
            }

            return Result.Success(setor.Map<SetorDto>());
        }
    }
}

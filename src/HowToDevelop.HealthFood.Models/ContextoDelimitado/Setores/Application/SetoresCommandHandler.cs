using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Setores;
using System.Collections.Generic;
using System.Linq;

namespace HowToDevelop.HealthFood.Application.Setores
{
    public class SetoresCommandHandler
        : IRequestHandler<IncluirSetorCommand, Result<SetorDto>>,
        IRequestHandler<AlterarDescricaoSetorCommand, Result<SetorDto>>,
        IRequestHandler<AdicionarMesaSetorCommand, Result<IEnumerable<MesaDto>>>,
        IRequestHandler<RemoverMesaSetorCommand, Result<IEnumerable<MesaDto>>>

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

        public async Task<Result<SetorDto>> Handle(AlterarDescricaoSetorCommand request, CancellationToken cancellationToken)
        {
            Maybe<Setor> setor = await _repositorio.ObterPorIdAsync(request.Id);
            if (setor.HasNoValue)
            {
                return Result.Failure<SetorDto>(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, request.Id));
            }
            
            var (_, isFailure, erro) = setor.Value.AlterarDescricaoSetor(request.Nome, request.Sigla);

            if (isFailure)
            {
                return Result.Failure<SetorDto>(erro);
            }
            _repositorio.Atualizar(setor.Value);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure<SetorDto>(SetoresConstantes.NaoFoiPossivelSalvarSetor);
            }

            return Result.Success(setor.Value.Map<SetorDto>());
        }

        public async Task<Result<IEnumerable<MesaDto>>> Handle(RemoverMesaSetorCommand request, CancellationToken cancellationToken)
        {
            Maybe<Setor> setor = await _repositorio.ObterComMesasPorIdAsync(request.SetorId);
            if (setor.HasNoValue)
            {
                return Result.Failure<IEnumerable<MesaDto>>(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, request.SetorId));
            }

            var (_, isFailure, erro) = setor.Value.RemoverMesa(request.Numeracao);
            if (isFailure)
            {
                return Result.Failure<IEnumerable<MesaDto>>(erro);
            }
            
            _repositorio.Atualizar(setor.Value);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<IEnumerable<MesaDto>>(SetoresConstantes.NaoFoiRemoverMesaSetor);
            }

            return Result.Success(setor.Value.Mesas.Select(s => s.Map<MesaDto>()));
        }

        public async Task<Result<IEnumerable<MesaDto>>> Handle(AdicionarMesaSetorCommand request, CancellationToken cancellationToken)
        {
            Maybe<Setor> setor = await _repositorio.ObterComMesasPorIdAsync(request.SetorId);
            if (setor.HasNoValue)
            {
                return Result.Failure<IEnumerable<MesaDto>>(string.Format(SetoresConstantes.NaoFoiEncontrarSetorInformado, request.SetorId));
            }

            var (_, isFailure, erro) = setor.Value.AdicionarMesa(request.Numeracao);
            if (isFailure)
            {
                return Result.Failure<IEnumerable<MesaDto>>(erro);
            }

            _repositorio.Atualizar(setor.Value);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<IEnumerable<MesaDto>>(SetoresConstantes.NaoFoiRemoverMesaSetor);
            }

            return Result.Success(setor.Value.Mesas.Select(s => s.Map<MesaDto>()));
        }
    }
}

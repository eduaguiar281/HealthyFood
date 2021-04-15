using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Garcons.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class GarconsCommandHandler : IRequestHandler<IncluirGarcomCommand, Result<GarcomDto>>,
        IRequestHandler<AlterarDadosPessoaisGarcomCommand, Result<GarcomDto>>,
        IRequestHandler<VincularSetorGarcomCommand, Result<IEnumerable<SetorAtendimentoDto>>>,
        IRequestHandler<RemoverSetorGarcomCommand, Result<IEnumerable<SetorAtendimentoDto>>>,
        IRequestHandler<ExcluirGarcomCommand, Result<int>>
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

        public async Task<Result<GarcomDto>> Handle(AlterarDadosPessoaisGarcomCommand request, CancellationToken cancellationToken)
        {
            Maybe<Garcom> garcom = await _repositorio.ObterPorIdAsync(request.RaizAgregacaoId);

            if (garcom.HasNoValue)
            {
                return Result.Failure<GarcomDto>(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, request.RaizAgregacaoId));
            }

            var (_, isFailure, erro) = garcom.Value.AlterarDadosPessoais(request.Nome, request.Apelido);

            if (isFailure)
            {
                return Result.Failure<GarcomDto>(erro);
            }

            _repositorio.Atualizar(garcom.Value);

            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure<GarcomDto>(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            }

            return Result.Success(garcom.Value.Map<GarcomDto>());
        }

        public async Task<Result<IEnumerable<SetorAtendimentoDto>>> Handle(VincularSetorGarcomCommand request, CancellationToken cancellationToken)
        {
            Maybe<Garcom> garcom = await _repositorio.ObterComSetoresAsync(request.RaizAgregacaoId, cancellationToken);

            if (garcom.HasNoValue)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, request.RaizAgregacaoId));
            }

            bool setorExiste = await _repositorio.SetorExiste(request.SetorId);
            if (!setorExiste)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, request.SetorId));
            }

            var (_, isFailure, erro) = garcom.Value.VincularSetor(request.SetorId);
            if (isFailure)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(erro);
            }
            _repositorio.Atualizar(garcom.Value);

            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            }

            return Result.Success(await _repositorio.ObterListaSetoresAsync(request.SetorId));
        }

        public async Task<Result<IEnumerable<SetorAtendimentoDto>>> Handle(RemoverSetorGarcomCommand request, CancellationToken cancellationToken)
        {
            Maybe<Garcom> garcom = await _repositorio.ObterComSetoresAsync(request.RaizAgregacaoId, cancellationToken);

            if (garcom.HasNoValue)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, request.RaizAgregacaoId));
            }

            var (_, isFailure, erro) = garcom.Value.RemoverVinculoDeSetor(request.SetorId);
            if (isFailure)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(erro);
            }
            _repositorio.Atualizar(garcom.Value);

            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<IEnumerable<SetorAtendimentoDto>>(GarconsConstantes.NaoFoiPossivelSalvarGarcom);
            }

            return Result.Success(await _repositorio.ObterListaSetoresAsync(request.SetorId));
        }

        public async Task<Result<int>> Handle(ExcluirGarcomCommand request, CancellationToken cancellationToken)
        {
            Maybe<Garcom> garcom = await _repositorio.ObterPorIdAsync(request.RaizAgregacaoId);
            if (garcom.HasNoValue)
            {
                return Result.Failure<int>(string.Format(GarconsConstantes.NaoFoiEncontrarGarcomInformado, request.RaizAgregacaoId));
            }

            _repositorio.Remover(garcom.Value);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<int>(GarconsConstantes.NaoFoiPossivelExcluirGarcom);
            }

            return Result.Success(request.RaizAgregacaoId);
        }
    }
}

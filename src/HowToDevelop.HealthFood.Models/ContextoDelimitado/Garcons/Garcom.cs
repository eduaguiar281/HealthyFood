using CSharpFunctionalExtensions;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HowToDevelop.HealthFood.Garcons
{
    public sealed class Garcom: RaizAgregacao<int>
    {
        [ExcludeFromCodeCoverage]
        private Garcom()
        {
            _setores = new List<SetorAtendimento>();
        }

        private Garcom(in Nome nome, in Apelido apelido, in int id)
            :base(id)
        {
            _setores = new List<SetorAtendimento>();
            Nome = nome;
            Apelido = apelido;
        }

        public Nome Nome { get; private set; }

        public Apelido Apelido { get; private set; }

        private readonly List<SetorAtendimento> _setores;
        public IReadOnlyCollection<SetorAtendimento> SetoresAtendimento => _setores.AsReadOnly();

        public Result VincularSetor(int setorId)
        {
            var setor = SetorAtendimento.Criar(setorId);

            var (_, isFailure, error) = Result.Combine(setor,
                _setores.AlgumNaoDeveSer(x => x.SetorId == setorId,
                GarconsConstantes.SetorJaFoiVinculadoAoGarcom));

            if (isFailure)
            {
                return Result.Failure(error);
            }

            _setores.Add(setor.Value);

            return Result.Success();
        }

        public Result RemoverVinculoDeSetor(int setorId)
        {
            var (_, isFailure, error) = Result.Combine(
                _setores.AlgumDeveSer(x => x.SetorId == setorId, string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, setorId)),
                _setores.DeveTerNoMinimo(2, GarconsConstantes.GarcomDeveTerNoMinimoUmSetorVinculado));

            if (isFailure)
            {
                return Result.Failure(error);
            }

            var setorRemover = _setores.FirstOrDefault(s => s.SetorId == setorId);

            _setores.Remove(setorRemover);

            return Result.Success();
        }

        public Result AlterarDadosPessoais(in string nome, in string apelido)
        {
            Result<Nome> nomeResult = Nome.Criar(nome);
            Result<Apelido> apelidoResult = Apelido.Criar(apelido);

            var (_, isFailure, error) = Result.Combine(nomeResult, apelidoResult);

            if (isFailure)
            {
                return Result.Failure<Garcom>(error);
            }

            Nome = nomeResult.Value;
            Apelido = apelidoResult.Value;
            return Result.Success();
        }

        public static Result<Garcom> Criar(in string nome, in string apelido, in int id = 0)
        {
            Result<Nome> nomeResult = Nome.Criar(nome);
            Result<Apelido> apelidoResult = Apelido.Criar(apelido);

            var (_, isFailure, error) = Result.Combine(nomeResult, apelidoResult);

            if (isFailure)
            {
                return Result.Failure<Garcom>(error);
            }

            return Result.Success(new Garcom(nomeResult.Value, apelidoResult.Value, id));
        }
    }
}

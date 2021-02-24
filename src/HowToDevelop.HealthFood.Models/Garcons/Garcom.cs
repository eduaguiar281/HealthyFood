using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.Interfaces;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HowToDevelop.HealthFood.Dominio.Garcons
{
    public sealed class Garcom: Entidade<int>, IRaizAgregacao
    {
        [ExcludeFromCodeCoverage]
        private Garcom()
        {
            _setores = new List<SetorAtendimento>();
        }

        private Garcom(in string nome, in string apelido, in int id)
            :base(id)
        {
            _setores = new List<SetorAtendimento>();
            Nome = nome;
            Apelido = apelido;
        }

        public string Nome { get; private set; }

        public string Apelido { get; private set; }

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
            var (_, isFailure, error) = ValidarDadosPessoais(nome, apelido);

            if (isFailure)
            {
                return Result.Failure(error);
            }
            Nome = nome;
            Apelido = apelido;
            return Result.Success();
        }

        private static Result ValidarDadosPessoais(in string nome, in string apelido)
        {
            return Result.Combine(nome.NaoDeveSerNuloOuVazio(GarconsConstantes.GarcomNomeEhObrigatorio),
                nome.TamanhoMenorOuIgual(GarconsConstantes.GarcomTamanhoMaximoNome, GarconsConstantes.NomeDeveTerAteCaracteres),
                apelido.TamanhoMenorOuIgual(GarconsConstantes.GarcomTamanhoMaximoApelido, GarconsConstantes.ApelidoDeveTerAteCaracteres));
        }

        public static Result<Garcom> Criar(in string nome, in string apelido, in int id = 0)
        {
            var (_, isFailure, error) = ValidarDadosPessoais(nome, apelido);

            if (isFailure)
            {
                return Result.Failure<Garcom>(error);
            }

            return Result.Success(new Garcom(nome, apelido, id));
        }
    }
}

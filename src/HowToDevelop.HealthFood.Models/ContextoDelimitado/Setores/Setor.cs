using CSharpFunctionalExtensions;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Setores.Application.Eventos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HowToDevelop.HealthFood.Setores
{
    public sealed class Setor : RaizAgregacao
    {
        [ExcludeFromCodeCoverage]
        private Setor()
        {
            _mesas = new List<Mesa>();
        }

        private Setor(in Nome nome, in Sigla sigla, in int id = 0)
            : base(id)
        {
            _mesas = new List<Mesa>();
            Nome = nome;
            Sigla = sigla;
        }

        private readonly List<Mesa> _mesas;

        public IReadOnlyCollection<Mesa> Mesas => _mesas.AsReadOnly();

        public Nome Nome { get; private set; }

        public Sigla Sigla { get; private set; }


        public Result AlterarDescricaoSetor(in string nome, in string sigla)
        {
            Result<Nome> nomeResult = Nome.Criar(nome);
            Result<Sigla> siglaResult = Sigla.Criar(sigla);

            var (_, isFailure, error) = Result.Combine(
                nomeResult, siglaResult,
                sigla.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoSiglaObrigatorio));

            if (isFailure)
            {
                return Result.Failure(error);
            }

            Nome = nomeResult.Value;
            Sigla = siglaResult.Value;
            return Result.Success();
        }

        public Result AdicionarMesa(ushort numero)
        {
            var mesa = Mesa.Criar(numero);

            var (_, isFailure, error) = Result.Combine(mesa,
                _mesas.AlgumNaoDeveSer(x => x.Numeracao == numero, string.Format(SetoresConstantes.JaExisteUmaMesaComEstaNumeracaoParaSetor, Id)));

            if (isFailure)
            {
                return Result.Failure(error);
            }

            _mesas.Add(mesa.Value);

            return Result.Success();
        }

        public Result RemoverMesa(ushort numero)
        {
            var (_, isFailure, error) = _mesas.AlgumDeveSer(x => x.Numeracao == numero, string.Format(SetoresConstantes.MesaInformadaNaoFoiLocalizada, numero));

            if (isFailure)
            {
                return Result.Failure(error);
            }

            var mesaRemove = _mesas.FirstOrDefault(s => s.Numeracao == numero);
            _mesas.Remove(mesaRemove);

            return Result.Success();

        }

        public static Result<Setor> Criar(in string nome, in string sigla, in int id = 0)
        {
            Result<Nome> nomeResult = Nome.Criar(nome);
            Result<Sigla> siglaResult = Sigla.Criar(sigla);

            var (_, isFailure, error) = Result.Combine(nomeResult, 
                siglaResult, 
                sigla.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoSiglaObrigatorio));

            if (isFailure)
            {
                return Result.Failure<Setor>(error);
            }

            var setor = new Setor(nomeResult.Value, siglaResult.Value, id);
            setor.AdicionarEvento(new SetorIncluidoEvent(nome, sigla));

            return Result.Success(setor);
        }
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HowToDevelop.HealthFood.Dominio.Setores
{
    public sealed class Setor: RaizAgregacao<int>
    {
        [ExcludeFromCodeCoverage]
        private Setor()
        {
            _mesas = new List<Mesa>();
        }

        private Setor(in string nome, in string sigla, in int id = 0)
            :base(id)
        {
            _mesas = new List<Mesa>();
            Nome = nome;
            Sigla = sigla;
        }

        private readonly List<Mesa> _mesas;
       
        public IReadOnlyCollection<Mesa> Mesas => _mesas.AsReadOnly();

        public string Nome { get; private set; }

        public string Sigla { get; private set; }


        public Result AlterarDescricaoSetor(in string nome, in string sigla)
        {
            var (_, isFailure, error) = ValidarDescricoes(nome, sigla);

            if (isFailure)
            {
                return Result.Failure(error);
            }

            Nome = nome;
            Sigla = sigla;

            return Result.Success();
        }

        public Result AdicionarMesa(ushort numero)
        {
            var mesa = Mesa.Criar(numero, 1);

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

        private static Result ValidarDescricoes(in string nome, in string sigla)
        {
            return Result.Combine(nome.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoNomeObrigatorio),
                nome.TamanhoMenorOuIgual(SetoresConstantes.SetorTamanhoMaximoNome, SetoresConstantes.SetorCampoNomeDeveTerAteNCaracteres),
                sigla.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoSiglaObrigatorio),
                sigla.TamanhoMenorOuIgual(SetoresConstantes.SetorTamanhoMaximoSigla, SetoresConstantes.SetorCampoSiglaDeveTerAteNCaracteres));
        }

        public static Result<Setor> Criar(in string nome, in string sigla, in int id = 0)
        {
            var (_, isFailure, error) =  ValidarDescricoes(nome, sigla);

            if (isFailure)
            {
                return Result.Failure<Setor>(error);
            }

            return Result.Success(new Setor(nome, sigla, id));
        }
    }
}

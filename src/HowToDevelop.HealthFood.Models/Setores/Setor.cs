using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Setores
{
    public sealed class Setor: Entidade<int>
    {
        public Setor()
        {
            _mesas = new List<Mesa>();
        }

        public Setor(int id)
            :base(id)
        {
            _mesas = new List<Mesa>();
        }

        private readonly List<Mesa> _mesas;
       
        public IReadOnlyCollection<Mesa> Mesas => _mesas.AsReadOnly();

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public Result AdicionarMesa(ushort numero)
        {
            var mesa = new Mesa()
            {
                Numeracao = numero
            };

            var (_, isFailure, error) = Result.Combine(mesa.EhValido(),
                _mesas.AlgumNaoDeveSer(x => x.Numeracao == numero, string.Format(SetoresConstantes.JaExisteUmaMesaComEstaNumeracaoParaSetor, Id)));

            if (isFailure)
                return Result.Failure(error);
            
            _mesas.Add(mesa);

            return Result.Success();
        }

        public Result RemoverMesa(ushort numero)
        {
            var (_, isFailure, error) = _mesas.AlgumDeveSer(x => x.Numeracao == numero, string.Format(SetoresConstantes.MesaInformadaNaoFoiLocalizada, numero));

            if (isFailure)
                return Result.Failure(error);

            var mesaRemove = _mesas.FirstOrDefault(s => s.Numeracao == numero);

            _mesas.Remove(mesaRemove);

            return Result.Success();

        }

        public override Result EhValido()
        {
            return Result.Combine(Nome.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoNomeObrigatorio),
                Nome.TamanhoMenorOuIgual(SetoresConstantes.SetorTamanhoMaximoNome, SetoresConstantes.SetorCampoNomeDeveTerAteNCaracteres),
                Sigla.NaoDeveSerNuloOuVazio(SetoresConstantes.SetorCampoSiglaObrigatorio),
                Sigla.TamanhoMenorOuIgual(SetoresConstantes.SetorTamanhoMaximoSigla, SetoresConstantes.SetorCampoSiglaDeveTerAteNCaracteres));

        }
    }
}

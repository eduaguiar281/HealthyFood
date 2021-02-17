using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using HowToDevelop.HealthFood.Dominio.Setores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Garcons
{
    public sealed class Garcom: Entidade<int>
    {
        public Garcom()
        {
            _setores = new List<SetorAtendimento>();
        }

        public Garcom(int id)
            :base(id)
        {
            _setores = new List<SetorAtendimento>();
        }

        public string Nome { get; set; }

        public string Apelido { get; set; }

        private readonly List<SetorAtendimento> _setores;
        public IReadOnlyCollection<SetorAtendimento> SetoresAtendimento => _setores.AsReadOnly();

        public Result AdicionarSetor(Setor setor)
        {
            var (_, isFailure, error) = Result.Combine(setor.EhValido(),
                _setores.AlgumNaoDeveSer(x => x.Setor.Id == setor.Id, GarconsConstantes.SetorJaFoiVinculadoAoGarcom));

            if (isFailure)
                return Result.Failure(error);

            _setores.Add(new SetorAtendimento(this, setor));

            return Result.Success();
        }

        public Result RemoverSetor(int setorId)
        {
            var (_, isFailure, error) = Result.Combine(
                _setores.AlgumDeveSer(x => x.Setor.Id == setorId, string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, setorId)),
                _setores.DeveTerNoMinimo(2, GarconsConstantes.GarcomDeveTerNoMinimoUmSetorVinculado));

            if (isFailure)
                return Result.Failure(error);

            var setorRemover = _setores.FirstOrDefault(s => s.Setor.Id == setorId);

            _setores.Remove(setorRemover);

            return Result.Success();
        }

        public override Result EhValido()
        {
            return Result.Combine(Nome.NaoDeveSerNuloOuVazio(GarconsConstantes.GarcomNomeEhObrigatorio),
                Nome.TamanhoMenorOuIgual(GarconsConstantes.GarcomTamanhoMaximoNome, GarconsConstantes.NomeDeveTerAteCaracteres),
                Apelido.TamanhoMenorOuIgual(GarconsConstantes.GarcomTamanhoMaximoApelido, GarconsConstantes.ApelidoDeveTerAteCaracteres),
                _setores.DeveTerNoMinimo(1, GarconsConstantes.GarcomDeveTerNoMinimoUmSetorVinculado));
        }
    }
}

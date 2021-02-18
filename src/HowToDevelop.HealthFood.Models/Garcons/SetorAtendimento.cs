using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using HowToDevelop.HealthFood.Dominio.Setores;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Garcons
{
    public sealed class SetorAtendimento: Entidade<int>
    {
        [ExcludeFromCodeCoverage]
        private SetorAtendimento()
        {

        }

        private SetorAtendimento(in int garcomId, in int setorId, in int id)
            :base(id)
        {
            _garcomId = garcomId;
            _setorId = setorId;
        }

        public int SetorId => _setorId;

        private readonly int _garcomId;
        private readonly int _setorId;


        public static Result<SetorAtendimento> Criar(in int garcomId, in int setorId, in int id = 0)
        {
            var (_, isFailure, error) = Result.Combine(
                garcomId.DeveSerMaiorQueZero(GarconsConstantes.GarcomIdNaoEhValido),
                setorId.DeveSerMaiorQueZero(GarconsConstantes.SetorIdNaoEhValido));

            if (isFailure)
            {
                return Result.Failure<SetorAtendimento>(error);
            }

            return Result.Success(new SetorAtendimento(garcomId, setorId, id));
        }
    }
}

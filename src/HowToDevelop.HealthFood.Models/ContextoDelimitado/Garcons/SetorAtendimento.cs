using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura.Garcons
{
    public sealed class SetorAtendimento: Entidade<int>
    {
        [ExcludeFromCodeCoverage]
        private SetorAtendimento()
        {

        }

        private SetorAtendimento(in int setorId, in int id)
            :base(id)
        {
            _setorId = setorId;
        }

        public int SetorId => _setorId;

        private readonly int _setorId;


        public static Result<SetorAtendimento> Criar(in int setorId, in int id = 0)
        {
            var (_, isFailure, error) = Result.Combine(
                setorId.DeveSerMaiorQueZero(GarconsConstantes.SetorIdNaoEhValido));

            if (isFailure)
            {
                return Result.Failure<SetorAtendimento>(error);
            }

            return Result.Success(new SetorAtendimento(setorId, id));
        }
    }
}

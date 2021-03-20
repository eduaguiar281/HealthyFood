using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Sigla : ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Sigla()
        {

        }

        private Sigla(string valor)
        {
            Valor = valor;
        }

        public string Valor { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }
        public override string ToString()
        {
            return Valor;
        }

        public static implicit operator string(Sigla n) => n.Valor;

        public static Result<Sigla> Criar(string sigla)
        {
            var (_, isFailure, erro) = sigla.TamanhoMenorOuIgual(SiglaConstantes.SiglaTamanhoMaximoPadrao,
                string.Format(SiglaConstantes.SiglaDeveTerNoMaximo, SiglaConstantes.SiglaTamanhoMaximoPadrao));

            if (isFailure)
            {
                return Result.Failure<Sigla>(erro);
            }

            return Result.Success(new Sigla(sigla));
        }
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Apelido : ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Apelido() { }
        private Apelido(string valor)
        {
            Valor = valor;
        }

        public string Valor { get; }

        public static Result<Apelido> Criar(in string apelido)
        {
            var result=  apelido.TamanhoMenorOuIgual(ApelidoConstantes.ApelidoTamanhoMaximoPadrao,
                string.Format(ApelidoConstantes.ApelidoDeveTerNoMaximo, ApelidoConstantes.ApelidoTamanhoMaximoPadrao));

            if (result.IsFailure)
            {
                return result.ConvertFailure<Apelido>();
            }

            return Result.Success(new Apelido(apelido));
        }

        public static implicit operator string(Apelido n) => n.Valor;

        public override string ToString()
        {
            return Valor;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor; 
        }
    }
}

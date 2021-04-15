using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Collections.Generic;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Descricao : ValueObject
    {
        protected Descricao()
        {

        }

        private Descricao(in string nome)
        {
            Valor = nome;
        }

        public string Valor { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString()
        {
            return Valor;
        }

        public static implicit operator string(Descricao n) => n.Valor;

        public static Result<Descricao> Criar(in string descricao)
        {
            var (_, isFailure, erro) = Result.Combine(
                string.IsNullOrEmpty(descricao) ? Result.Success() : descricao.DeveTerNoMinimo(DescricaoConstantes.DescricaoTamanhoMinimoPadrao,
                string.Format(DescricaoConstantes.DescricaoDeveTerNoMinimo, DescricaoConstantes.DescricaoTamanhoMinimoPadrao)),
                descricao.TamanhoMenorOuIgual(DescricaoConstantes.DescricaoTamanhoMaximoPadrao,
                string.Format(DescricaoConstantes.DescricaoDeveTerNoMaximo, DescricaoConstantes.DescricaoTamanhoMaximoPadrao)),
                descricao.NaoDeveSerNuloOuVazio(DescricaoConstantes.DescricaoEhObrigatorio));

            if (isFailure)
                return Result.Failure<Descricao>(erro);

            return Result.Success(new Descricao(descricao));
        }
    }
}

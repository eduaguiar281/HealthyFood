using CSharpFunctionalExtensions;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor
{
    public class Nome: ValueObject
    {
        [ExcludeFromCodeCoverage]
        protected Nome()
        {

        }

        private Nome(in string nome)
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

        public static implicit operator string(Nome n) => n.Valor;

        public static Result<Nome> Criar(in string nome)
        {
            var (_, isFailure, erro) = Result.Combine(
                string.IsNullOrEmpty(nome) ? Result.Success() : nome.DeveTerNoMinimo(NomeConstantes.NomeTamanhoMinimoPadrao,
                string.Format(NomeConstantes.NomeDeveTerNoMinimo, NomeConstantes.NomeTamanhoMinimoPadrao)),
                nome.TamanhoMenorOuIgual(NomeConstantes.NomeTamanhoMaximoPadrao,
                string.Format(NomeConstantes.NomeDeveTerNoMaximo, NomeConstantes.NomeTamanhoMaximoPadrao)),
                nome.NaoDeveSerNuloOuVazio(NomeConstantes.NomeEhObrigatorio));
            
            if (isFailure)
                return Result.Failure<Nome>(erro);
            
            return Result.Success(new Nome(nome));
        }
    }
}

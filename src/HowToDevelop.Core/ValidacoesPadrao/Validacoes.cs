using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowToDevelop.Core.ValidacoesPadrao
{
    public static class Validacoes
    {
        public static Result NaoDeveSerNuloOuVazio(this string valor, in string motivo)
        {
            return string.IsNullOrEmpty(valor)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result NaoDeveSerNulo<T>(this T valor, in string motivo) where T : class
        {
            return valor == null
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveTerNoMinimo<T>(this IEnumerable<T> valor, in int quantidade, in string motivo)
        {
            return valor.Count() < quantidade
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveSerMaiorQue(this DateTime valor, in DateTime desejado, in string motivo)
        {
            return valor <= desejado
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveSerMaiorQue(this int valor, in int desejado, in string motivo)
        {
            return valor <= desejado
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveSerMaiorQue(this decimal valor, in int desejado, in string motivo)
        {
            return valor <= desejado
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveSerMenorOuIgualQue(this decimal valor, in decimal desejado, in string motivo)
        {
            return valor <= desejado
                ? Result.Success()
                : Result.Failure(motivo);
        }

        public static Result DeveSerMaiorQueZero(this int valor, in string motivo)
            => valor.DeveSerMaiorQue(0, motivo);

        public static Result DeveSerMaiorQueZero(this decimal valor, in string motivo)
            => valor.DeveSerMaiorQue(0, motivo);

        public static Result NaoDeveSerVazio<T>(this IEnumerable<T> valor, string motivo)
        {
            return !valor.Any()
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result DeveSer<T>(this T valor, in T desejado, in string motivo) where T : struct
        {
            return !valor.Equals(desejado)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result NaoDeveSer<T>(this T valor, in T desejado, in string motivo) where T : struct
        {
            return valor.Equals(desejado)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result NaoDeveSer(this ushort valor, in ushort desejado, in string motivo)
        {
            return valor.Equals(desejado)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result TodosDevemSer<T>(this IEnumerable<T> valor, in Func<T, bool> condicao, string motivo)
        {
            return !valor.All(condicao)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result AlgumDeveSer<T>(this IEnumerable<T> valor, in Func<T, bool> condicao, string motivo)
        {
            return !valor.Any(condicao)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result AlgumNaoDeveSer<T>(this IEnumerable<T> valor, in Func<T, bool> condicao, string motivo)
        {
            return valor.Any(condicao)
                ? Result.Failure(motivo)
                : Result.Success();
        }

        public static Result TamanhoMenorOuIgual(this string valor, int tamanho, string motivo)
        {
            if ((string.IsNullOrEmpty(valor)) || (valor.Length <= tamanho))
                return Result.Success();

            return Result.Failure(motivo);
        }
    }
}

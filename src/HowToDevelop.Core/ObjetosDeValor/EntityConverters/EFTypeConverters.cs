using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor.EntityConverters
{
    [ExcludeFromCodeCoverage]
    public static class EFTypeConverters
    {
        public static readonly ValueConverter<Percentual, decimal> PercentualConverter = new(
            percentual => percentual.Valor,
            percentualValor => Percentual.Criar(percentualValor).Value);

        public static readonly ValueConverter<Preco, decimal> PrecoConverter = new(
            preco => preco.Valor,
            precoValor => new Preco(precoValor));

        public static readonly ValueConverter<Quantidade, int> QuantidadeConverter = new(
            quantidade => quantidade.Valor,
            quantidadeValor => new Quantidade(quantidadeValor));

        public static readonly ValueConverter<Total, decimal> TotalConverter = new(
            total => total.Valor,
            totalValor => new Total(totalValor));

        public static readonly ValueConverter<Nome, string> NomeConverter = new(
            nome => nome.Valor,
            nomeValor => Nome.Criar(nomeValor).Value);

        public static readonly ValueConverter<Apelido, string> ApelidoConverter = new(
            apelido => apelido.Valor,
            apelidoValor => Apelido.Criar(apelidoValor).Value);

        public static readonly ValueConverter<Sigla, string> SiglaConverter = new(
            sigla => sigla.Valor,
            siglaValor => Sigla.Criar(siglaValor).Value);

        public static readonly ValueConverter<Descricao, string> DescricaoConverter = new(
            descricao => descricao.Valor,
            descricaoValor => Descricao.Criar(descricaoValor).Value);
    }
}

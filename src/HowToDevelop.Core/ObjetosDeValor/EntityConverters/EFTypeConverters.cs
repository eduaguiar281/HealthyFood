using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.Core.ObjetosDeValor.EntityConverters
{
    [ExcludeFromCodeCoverage]
    public static class EFTypeConverters
    {
        public static readonly ValueConverter<Percentual, decimal> PercentualConverter = new ValueConverter<Percentual, decimal>(
            percentual => percentual.Valor,
            percentualValor => Percentual.Criar(percentualValor).Value);

        public static readonly ValueConverter<Preco, decimal> PrecoConverter = new ValueConverter<Preco, decimal>(
            preco => preco.Valor,
            precoValor => new Preco(precoValor));

        public static readonly ValueConverter<Quantidade, int> QuantidadeConverter = new ValueConverter<Quantidade, int>(
            quantidade => quantidade.Valor,
            quantidadeValor => new Quantidade(quantidadeValor));

        public static readonly ValueConverter<Total, decimal> TotalConverter = new ValueConverter<Total, decimal>(
            total => total.Valor,
            totalValor => new Total(totalValor));

        public static readonly ValueConverter<Nome, string> NomeConverter = new ValueConverter<Nome, string>(
            nome => nome.Valor,
            nomeValor => Nome.Criar(nomeValor).Value);

        public static readonly ValueConverter<Apelido, string> ApelidoConverter = new ValueConverter<Apelido, string>(
            apelido => apelido.Valor,
            apelidoValor => Apelido.Criar(apelidoValor).Value);

    }
}

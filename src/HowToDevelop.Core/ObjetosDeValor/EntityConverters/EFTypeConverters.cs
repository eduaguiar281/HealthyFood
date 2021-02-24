using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HowToDevelop.Core.ObjetosDeValor.EntityConverters
{
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
            quantidade => quantidade.Valor,
            quantidadeValor => new Total(quantidadeValor));
    }
}

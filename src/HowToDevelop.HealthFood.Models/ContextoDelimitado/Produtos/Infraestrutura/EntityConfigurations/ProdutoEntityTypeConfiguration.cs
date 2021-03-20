using HowToDevelop.Core.ObjetosDeValor.EntityConverters;
using HowToDevelop.HealthFood.Infraestrutura.Produtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    [ExcludeFromCodeCoverage]
    public class ProdutoEntityTypeConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.CodigoBarras)
                .HasMaxLength(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras);

            builder.Property(p => p.Descricao)
                .HasMaxLength(ProdutosConstantes.ProdutoTamanhoCampoDescricao);

            builder.Property(p => p.Preco)
                .HasConversion(EFTypeConverters.PrecoConverter);

            builder.Property(e => e.TipoProduto)
                .HasColumnName("TipoProduto")
                .HasMaxLength(40).HasConversion(
                  v => v.ToString(),
                  v => (TipoProduto)Enum.Parse(typeof(TipoProduto), v));
        }
    }
}

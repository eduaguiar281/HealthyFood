﻿using HowToDevelop.Core.ObjetosDeValor.EntityConverters;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Produtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class ItensPedidoEntityTypeConfiguration : IEntityTypeConfiguration<ItensPedido>
    {
        public void Configure(EntityTypeBuilder<ItensPedido> builder)
        {
            builder.ToTable("ItensPedido");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.ProdutoId);
            builder.HasOne<Produto>()
                .WithMany()
                .HasForeignKey(fk => fk.ProdutoId);

            builder.Property(p => p.Quantidade)
                .HasConversion(EFTypeConverters.QuantidadeConverter);

            builder.Property(p => p.Preco)
                .HasConversion(EFTypeConverters.PrecoConverter);

            builder.Property(p => p.TotalItem)
                .HasConversion(EFTypeConverters.TotalConverter);
        }
    }
}

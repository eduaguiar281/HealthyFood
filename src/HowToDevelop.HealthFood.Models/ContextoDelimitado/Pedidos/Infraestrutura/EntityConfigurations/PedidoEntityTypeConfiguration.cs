using HowToDevelop.Core.ObjetosDeValor.EntityConverters;
using HowToDevelop.HealthFood.Dominio.Garcons;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Pedidos.ObjetosDeValor;
using HowToDevelop.HealthFood.Dominio.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class PedidoEntityTypeConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.MesaId);
            builder.HasOne<Mesa>()
               .WithMany()
               .HasForeignKey(fk => fk.MesaId);

            builder.Property(p => p.GarcomId);
            builder.HasOne<Garcom>()
               .WithMany()
               .HasForeignKey(fk => fk.GarcomId);

            builder.Property(p => p.NomeCliente)
                .HasMaxLength(PedidosConstantes.PedidosTamanhoNomeCliente)
                .IsRequired(false);

            builder.Property(e => e.Status)
                .HasColumnName("Status")
                .HasMaxLength(40).HasConversion(
                  v => v.ToString(),
                  v => (StatusPedido)Enum.Parse(typeof(StatusPedido), v));

            builder.Property(p => p.Total)
                .HasConversion(EFTypeConverters.TotalConverter);

            builder.OwnsOne(p => p.Desconto, descontoBuilder =>
            {
                descontoBuilder.Property(p => p.Percentual)
                    .HasColumnName("DescontoPercentual")
                    .HasConversion(EFTypeConverters.PercentualConverter);

                descontoBuilder.Property(p => p.Valor)
                    .HasColumnName("DescontoValor");

                descontoBuilder.Property(p => p.BaseCalculo)
                    .HasColumnName("DescontoBaseCalculo");

                descontoBuilder.Property(p => p.TipoDescontoPedido)
                    .HasConversion(
                       v => v.ToString(),
                       v => (TipoDesconto)Enum.Parse(typeof(TipoDesconto), v));
            });

            builder.OwnsOne(p => p.Comissao, comissaoBuilder =>
             {
                 comissaoBuilder.Property(p => p.BaseCalculo)
                    .HasColumnName("ComissaoBaseCalculo");

                 comissaoBuilder.Property(p => p.Percentual)
                     .HasColumnName("ComissaoPercentual")
                     .HasConversion(EFTypeConverters.PercentualConverter);

                 comissaoBuilder.Property(p => p.Gorjeta)
                    .HasColumnName("ComissaoGorjeta");

                 comissaoBuilder.Property(p => p.Valor)
                    .HasColumnName("ComissaoValor");

                 comissaoBuilder.Property(p => p.Total)
                    .HasColumnName("ComissaoTotal")
                    .HasConversion(EFTypeConverters.TotalConverter);
             });

        }
    }
}

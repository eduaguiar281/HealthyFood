﻿using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ObjetosDeValor.EntityConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Garcons.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class GarcomEntityTypeConfiguration : IEntityTypeConfiguration<Garcom>
    {
        public void Configure(EntityTypeBuilder<Garcom> builder)
        {
            builder.ToTable("Garcons");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasConversion(EFTypeConverters.NomeConverter)
                .HasMaxLength(NomeConstantes.NomeTamanhoMaximoPadrao);
            
            builder.Property(p => p.Apelido)
                .HasConversion(EFTypeConverters.ApelidoConverter)
                .HasMaxLength(ApelidoConstantes.ApelidoTamanhoMaximoPadrao);

            builder.HasMany(s => s.SetoresAtendimento)
               .WithOne()
               .HasForeignKey("GarcomId")
               .OnDelete(DeleteBehavior.Cascade)
               .Metadata
               .PrincipalToDependent
               .SetField("_setores");

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Garcons.Infraestrutura.EntityConfigurations
{
    public class GarcomEntityTypeConfiguration : IEntityTypeConfiguration<Garcom>
    {
        public void Configure(EntityTypeBuilder<Garcom> builder)
        {
            builder.ToTable("Garcons");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(GarconsConstantes.GarcomTamanhoMaximoNome);
            
            builder.Property(p => p.Apelido)
                .HasMaxLength(GarconsConstantes.GarcomTamanhoMaximoApelido);

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

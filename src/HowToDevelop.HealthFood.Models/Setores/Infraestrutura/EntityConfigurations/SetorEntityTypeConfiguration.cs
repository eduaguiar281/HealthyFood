using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Setores.Infraestrutura.EntityConfigurations
{
    public class SetorEntityTypeConfiguration : IEntityTypeConfiguration<Setor>
    {
        public void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.ToTable("Setores");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(SetoresConstantes.SetorTamanhoMaximoNome);

            builder.Property(p => p.Sigla)
                .HasMaxLength(SetoresConstantes.SetorTamanhoMaximoSigla);

            builder.HasMany(s => s.Mesas)
               .WithOne()
               .HasForeignKey("SetorId")
               .OnDelete(DeleteBehavior.Cascade)
               .Metadata
               .PrincipalToDependent
               .SetField("_mesas");

        }
    }
}

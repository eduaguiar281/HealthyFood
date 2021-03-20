using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    [ExcludeFromCodeCoverage]
    public class GarcomEntityTypeConfiguration : IEntityTypeConfiguration<Garcom>
    {
        public void Configure(EntityTypeBuilder<Garcom> builder)
        {
            builder.ToTable("Garcons");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(NomeConstantes.NomeTamanhoMaximoPadrao);
            
            builder.Property(p => p.Apelido)
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

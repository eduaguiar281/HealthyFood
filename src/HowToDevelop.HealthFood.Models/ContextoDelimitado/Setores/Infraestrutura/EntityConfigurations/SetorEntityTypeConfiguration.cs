using HowToDevelop.HealthFood.Dominio.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    [ExcludeFromCodeCoverage]
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

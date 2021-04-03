using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ObjetosDeValor.EntityConverters;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    [ExcludeFromCodeCoverage]
    public class SetorEntityTypeConfiguration : IEntityTypeConfiguration<HealthFood.Setores.Setor>
    {
        public void Configure(EntityTypeBuilder<HealthFood.Setores.Setor> builder)
        {
            builder.ToTable("Setores");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasConversion(EFTypeConverters.NomeConverter)
                .HasMaxLength(NomeConstantes.NomeTamanhoMaximoPadrao);

            builder.Property(p => p.Sigla)
                .HasConversion(EFTypeConverters.SiglaConverter)
                .HasMaxLength(SiglaConstantes.SiglaTamanhoMaximoPadrao);

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

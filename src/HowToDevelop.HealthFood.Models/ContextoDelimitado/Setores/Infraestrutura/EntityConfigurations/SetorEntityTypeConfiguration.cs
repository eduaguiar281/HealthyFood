using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura.Setores
{
    [ExcludeFromCodeCoverage]
    public class SetorEntityTypeConfiguration : IEntityTypeConfiguration<Setor>
    {
        public void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.ToTable("Setores");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(NomeConstantes.NomeTamanhoMaximoPadrao);

            builder.Property(p => p.Sigla)
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

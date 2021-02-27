using HowToDevelop.HealthFood.Dominio.Garcons;
using HowToDevelop.HealthFood.Dominio.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class SetorAtendimentoEntityTypeConfiguration : IEntityTypeConfiguration<SetorAtendimento>
    {
        public void Configure(EntityTypeBuilder<SetorAtendimento> builder)
        {
            builder.ToTable("SetoresAtendimento");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.SetorId);

            builder.HasOne<Setor>()
               .WithMany()
               .HasForeignKey(fk=> fk.SetorId);
        }
    }
}

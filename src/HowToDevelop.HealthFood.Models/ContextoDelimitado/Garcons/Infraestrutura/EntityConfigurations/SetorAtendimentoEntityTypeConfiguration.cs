using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Garcons.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class SetorAtendimentoEntityTypeConfiguration : IEntityTypeConfiguration<SetorAtendimento>
    {
        public void Configure(EntityTypeBuilder<SetorAtendimento> builder)
        {
            builder.ToTable("SetoresAtendimento");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.SetorId);

            builder.HasOne<HealthFood.Setores.Setor>()
               .WithMany()
               .HasForeignKey(fk=> fk.SetorId);
        }
    }
}

using HowToDevelop.HealthFood.Dominio.Setores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class MesaEntityTypeConfiguration : IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> builder)
        {
            builder.ToTable("Mesas");
            builder.HasKey(s => s.Id);

            builder.Property(p => p.Numeracao);
        }
    }
}

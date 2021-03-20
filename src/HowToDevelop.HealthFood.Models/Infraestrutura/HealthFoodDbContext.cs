using HowToDevelop.Core.Comunicacao;
using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using HowToDevelop.HealthFood.Infraestrutura.Pedidos;
using HowToDevelop.HealthFood.Infraestrutura.Produtos;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public class HealthFoodDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public HealthFoodDbContext(DbContextOptions<HealthFoodDbContext> options,
            IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Garcom> Garcons { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Setor> Setores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GarcomEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SetorAtendimentoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PedidoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItensPedidoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ProdutoEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new SetorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MesaEntityTypeConfiguration());

            modelBuilder.Ignore<Evento>();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await CommitAsync(cancellationToken) > 0;
        }


        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCriacao") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCriacao").CurrentValue = DateTime.UtcNow;
                    entry.Property("DataAlteracao").IsModified = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataAlteracao").CurrentValue = DateTime.UtcNow;
                    entry.Property("DataCriacao").IsModified = false;
                }
            }

            int result = await base.SaveChangesAsync();
            if (result > 0)
            {
                await _mediatorHandler.PublicarEventos(this);
            }

            return result;
        }
    }
}

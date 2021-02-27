using HowToDevelop.Core;
using HowToDevelop.Core.Comunicacao;
using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Dominio.Garcons;
using HowToDevelop.HealthFood.Dominio.Pedidos;
using HowToDevelop.HealthFood.Dominio.Produtos;
using HowToDevelop.HealthFood.Dominio.Setores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    public class HealthFoodDbContext: DbContext, IUnitOfWork
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
            return await Commit(cancellationToken) > 0;
        }


        public async Task<int> Commit(CancellationToken cancellationToken = default)
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

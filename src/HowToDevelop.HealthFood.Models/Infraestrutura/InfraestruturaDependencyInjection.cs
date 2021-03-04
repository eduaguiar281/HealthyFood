using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    public static class InfraestruturaDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // database
            var connectionString = configuration.GetConnectionString("HealthFoodConnection");

            services.AddDbContext<HealthFoodDbContext>(options =>
                options.UseSqlServer(connectionString));


            return services;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public static class InfraestruturaDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // database
            var connectionString = configuration.GetConnectionString("HealthFoodConnection");

            services.AddDbContext<HealthFoodDbContext>(options =>
                options.UseSqlServer(connectionString));

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new DomainModelToDtoMappingProfile()));
            AutoMapperConfiguration.Init(config);
            services.AddSingleton(AutoMapperConfiguration.Mapper);

            return services;
        }
    }
}

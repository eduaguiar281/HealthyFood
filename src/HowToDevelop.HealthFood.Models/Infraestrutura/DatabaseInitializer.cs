using Microsoft.EntityFrameworkCore;
using System;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    public static class DatabaseInitializer
    {
        public static void Initialize(HealthFoodDbContext context)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                context.Database.Migrate();
            }
        }

    }
}

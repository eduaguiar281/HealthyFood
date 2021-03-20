using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Infraestrutura
{
    [ExcludeFromCodeCoverage]
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

using Core.API.Data;

namespace Core.API.Extensions;
public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<LetsGameContext>("CoreDB");

        // Seed the database with default values
        //builder.Services.AddMigration<LetsGameContext, LetsGameDbSeed>();
    }
}

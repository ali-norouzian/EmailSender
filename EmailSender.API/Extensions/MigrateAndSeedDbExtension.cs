using EmailSender.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.API.Extensions
{
    public static class MigrateAndSeedDbExtension
    {
        public static async Task<WebApplication> MigrateAndSeedDb(this WebApplication app)
        {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    //var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    await context.Database.MigrateAsync();
                    //await Seed.SeedData(context, userManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occured during migration database.");
                }
            }

            return app;
        }
    }
}

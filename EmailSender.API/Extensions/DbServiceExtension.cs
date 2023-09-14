using EmailSender.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.API.Extensions
{
    public static class DbServiceExtension
    {
        public static IServiceCollection AddDbService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });


            return services;
        }

    }
}

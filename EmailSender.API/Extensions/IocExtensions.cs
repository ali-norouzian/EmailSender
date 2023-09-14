using EmailSender.Core.Interfaces;
using EmailSender.Core.Profiles;
using EmailSender.Core.Services;

namespace EmailSender.API.Extensions
{
    public static class IocExtensions
    {
        public static IServiceCollection AddIocService(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}

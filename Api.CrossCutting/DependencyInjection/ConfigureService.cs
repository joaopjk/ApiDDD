using Api.Domain.Interfaces.Services.User;
using Api.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService (IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService,UserService>(); // Para cada chamada será criado uma nova instância
            serviceCollection.AddTransient<ILoginService, LoginService>();
        }
    }
}
using Kubonia.Client.Navigation;
using Kubonia.Client.Services;
using Kubonia.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Client.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:5215/")
        });

        services.AddSingleton<IKubeServerClient, KubeServerClient>();
        services.AddSingleton<ViewFactory>();

        services.AddTransient<OverviewView>();
        services.AddTransient<PodsView>();

        return services;
    }
}

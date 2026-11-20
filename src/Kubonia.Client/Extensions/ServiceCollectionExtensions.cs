using Kubonia.Client.Navigation;
using Kubonia.Client.Views;
using Kubonia.Kube;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Client.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddKubernetesAccessor();

        services.AddSingleton<ViewFactory>();

        services.AddTransient<PodsView>();

        return services;
    }
}

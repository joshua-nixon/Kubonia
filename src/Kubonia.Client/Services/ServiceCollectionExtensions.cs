using System;
using System.Net.Http;
using Kubonia.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Client.Services;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:5215/")
        });
        services.AddSingleton<IKubeServerClient, KubeServerClient>();
        services.AddTransient<PodsView>();
        services.AddSingleton<Func<PodsView>>(provider => provider.GetRequiredService<PodsView>);

        return services;
    }
}

using k8s;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Kube
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddKubernetesAccessor()
            {
                services.AddSingleton<IKubernetes>(_ =>
                {
                    var configuration = KubernetesClientConfiguration.BuildConfigFromConfigFile();

                    return new Kubernetes(configuration);
                });

                services.AddSingleton<IKubernetesService, KubernetesService>();

                return services;
            }
        }
    }
}

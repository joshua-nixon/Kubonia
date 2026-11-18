using k8s;

namespace Kubonia.Server.K8s
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

                services.AddSingleton<IKubernetesAccessor, KubernetesAccessor>();

                return services;
            }
        }
    }
}

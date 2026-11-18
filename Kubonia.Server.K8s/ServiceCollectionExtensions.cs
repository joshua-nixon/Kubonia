using System;
using System.Collections.Generic;
using System.Text;
using k8s;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Server.K8s
{
    public static class ServiceCollectionExtensions
    {
        extension (IServiceCollection services)
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

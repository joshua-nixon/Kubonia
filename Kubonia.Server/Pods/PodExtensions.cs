using k8s.Models;
using Kubonia.Shared.Common;
using Kubonia.Shared.Contracts.Pods;

namespace Kubonia.Server.Pods
{
    internal static class PodExtensions
    {
        extension (IServiceCollection services)
        {
            public IServiceCollection AddPodServices()
            {
                services.AddSingleton<IPodService, KubernetesPodService>();

                return services;
            }
        }

        extension (V1Pod pod)
        {
            public PodDTO ToPodDTO()
            {
                Guard.IsNotNull(pod.Metadata);
                Guard.IsNotNull(pod.Status);

                var containerStatuses = pod.Status.ContainerStatuses ?? [];
                var containerCount = pod.Spec.Containers?.Count ?? 0;

                DateTimeOffset? createdAt = pod.Metadata.CreationTimestamp is null
                    ? null
                    : new DateTimeOffset(DateTime.SpecifyKind(pod.Metadata.CreationTimestamp.Value, DateTimeKind.Utc));

                return new PodDTO(
                    pod.Metadata.Name,
                    pod.Metadata.NamespaceProperty,
                    pod.Status.Phase ?? PodStatus.Unknown,
                    NodeName: pod.Spec.NodeName ?? string.Empty,
                    PodIp: pod.Status.PodIP,
                    CreatedAt: createdAt,
                    ReadyContainers: containerStatuses.Count(status => status.Ready),
                    ContainerCount: containerCount,
                    RestartCount: containerStatuses.Sum(status => status.RestartCount)
                );
            }
        }
    }
}

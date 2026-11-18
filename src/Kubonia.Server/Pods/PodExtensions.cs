using k8s.Models;
using Kubonia.Shared.Common;
using Kubonia.Shared.Pods;

namespace Kubonia.Server.Pods
{
    internal static class PodExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddPodServices()
            {
                services.AddSingleton<IPodService, KubernetesPodService>();

                return services;
            }
        }

        extension(V1Pod pod)
        {
            public PodDTO ToPodDTO()
            {
                Guard.IsNotNull(pod.Metadata);
                Guard.IsNotNull(pod.Status);

                var containerStatuses = pod.Status.ContainerStatuses ?? [];
                var containers = (pod.Spec.Containers ?? [])
                    .Select(container => new ContainerDTO(
                        container.Name,
                        containerStatuses.FirstOrDefault(status => status.Name == container.Name)?.Ready == true
                            ? ContainerStatus.Ready
                            : ContainerStatus.NotReady))
                    .ToArray();

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
                    ReadyContainers: containers.Count(container => container.Status == ContainerStatus.Ready),
                    RestartCount: containerStatuses.Sum(status => status.RestartCount),
                    Containers: containers
                );
            }
        }
    }
}

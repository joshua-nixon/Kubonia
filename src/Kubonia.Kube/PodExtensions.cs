using k8s.Models;
using Kubonia.Shared.Containers;
using Kubonia.Shared.Pods;

namespace Kubonia.Kube
{
    internal static class PodExtensions
    {

        extension(V1Pod pod)
        {
            public PodDTO ToPodDTO()
            {
                var containerStatuses = pod.Status.ContainerStatuses
                    .ToDictionary(x => x.Name, x => x);

                var containers = pod.Spec.Containers
                    .Select(container =>
                        new ContainerDTO(
                            container.Name,
                            containerStatuses[container.Name].Ready ? ContainerStatus.Ready : ContainerStatus.NotReady,
                            containerStatuses[container.Name].RestartCount
                        )
                    ).ToArray();

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
                    RestartCount: containers.Sum(status => status.RestartCount),
                    Containers: containers
                );
            }
        }
    }
}

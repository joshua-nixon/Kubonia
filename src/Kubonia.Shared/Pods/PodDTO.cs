namespace Kubonia.Shared.Pods;

public sealed record PodDTO(
    string Name,
    string Namespace,
    string Phase,
    string NodeName,
    string PodIp,
    DateTimeOffset? CreatedAt,
    int ReadyContainers,
    long RestartCount,
    IReadOnlyList<ContainerDTO> Containers
);

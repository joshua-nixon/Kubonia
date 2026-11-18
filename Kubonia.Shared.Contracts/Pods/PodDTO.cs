namespace Kubonia.Shared.Contracts.Pods;

public sealed record PodDTO(
    string Name,
    string Namespace,
    string Phase,
    string NodeName,
    string PodIp,
    DateTimeOffset? CreatedAt,
    int ReadyContainers,
    int ContainerCount,
    long RestartCount
);

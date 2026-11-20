namespace Kubonia.Shared.Containers;

public sealed record ContainerDTO(
    string Name,
    string Status,
    int RestartCount
);

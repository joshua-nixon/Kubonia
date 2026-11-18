namespace Kubonia.Shared.Pods;

public sealed record ContainerDTO(
    string Name, 
    string Status,
    int RestartCount
);

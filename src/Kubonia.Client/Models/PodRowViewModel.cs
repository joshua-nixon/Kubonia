using Kubonia.Shared.Containers;
using Kubonia.Shared.Pods;

namespace Kubonia.Client.Models;

public sealed record PodRowViewModel(
    string Name,
    string Namespace,
    string Phase,
    string NodeName,
    string PodIp,
    long RestartCount,
    string Age,
    DateTimeOffset? CreatedAt,
    IReadOnlyList<ContainerDTO> Containers
)
{
    public int ContainerCount => Containers.Count;

    public IReadOnlyList<ContainerIndicatorViewModel> ContainerIndicators { get; } =
        [.. Containers.Select(container => new ContainerIndicatorViewModel(container.Name, container.Status))];

    public bool IsSuccessStatus =>
        string.Equals(Phase, PodStatus.Running, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Succeeded, StringComparison.OrdinalIgnoreCase);

    public bool IsWarningStatus =>
        string.Equals(Phase, PodStatus.Pending, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Unknown, StringComparison.OrdinalIgnoreCase);

    public bool IsDangerStatus =>
        string.Equals(Phase, PodStatus.Failed, StringComparison.OrdinalIgnoreCase);
}

public sealed record ContainerIndicatorViewModel(string Name, string Status)
{
    public bool IsReady => Status == ContainerStatus.Ready;

    public string Tooltip => Name;
}

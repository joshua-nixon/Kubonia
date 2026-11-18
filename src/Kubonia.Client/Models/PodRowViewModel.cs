using Kubonia.Shared.Pods;

namespace Kubonia.Client.Models;

public sealed record PodRowViewModel(
    string Name,
    string Namespace,
    string Phase,
    string NodeName,
    string PodIp,
    string Ready,
    int ReadyContainers,
    int ContainerCount,
    long RestartCount,
    string Age,
    DateTimeOffset? CreatedAt
)
{
    public IReadOnlyList<bool> ContainerIndicators { get; } =
        Enumerable.Range(0, Math.Max(ContainerCount, 0))
            .Select(index => index < ReadyContainers)
            .ToArray();

    public bool IsSuccessStatus =>
        string.Equals(Phase, PodStatus.Running, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Succeeded, StringComparison.OrdinalIgnoreCase);

    public bool IsWarningStatus =>
        string.Equals(Phase, PodStatus.Pending, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Unknown, StringComparison.OrdinalIgnoreCase);

    public bool IsDangerStatus =>
        string.Equals(Phase, PodStatus.Failed, StringComparison.OrdinalIgnoreCase);
}

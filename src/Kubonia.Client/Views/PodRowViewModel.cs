using Kubonia.Shared.Pods;

namespace Kubonia.Client.Views;

public sealed record PodRowViewModel(
    string Name,
    string Namespace,
    string Phase,
    string NodeName,
    string PodIp,
    string Ready,
    long RestartCount,
    string Age,
    DateTimeOffset? CreatedAt
)
{
    public bool IsSuccessStatus =>
        string.Equals(Phase, PodStatus.Running, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Succeeded, StringComparison.OrdinalIgnoreCase);

    public bool IsWarningStatus =>
        string.Equals(Phase, PodStatus.Pending, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Phase, PodStatus.Unknown, StringComparison.OrdinalIgnoreCase);

    public bool IsDangerStatus =>
        string.Equals(Phase, PodStatus.Failed, StringComparison.OrdinalIgnoreCase);
}

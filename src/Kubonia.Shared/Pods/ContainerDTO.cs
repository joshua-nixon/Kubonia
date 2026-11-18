namespace Kubonia.Shared.Pods;

/// <summary>Container readiness; missing Kubernetes status is reported as NotReady.</summary>
public sealed record ContainerDTO(string Name, string Status);

public static class ContainerStatus
{
    public const string Ready = "Ready";
    public const string NotReady = "NotReady";
}

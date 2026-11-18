using System;

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
    DateTimeOffset? CreatedAt);

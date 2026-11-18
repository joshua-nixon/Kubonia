using Avalonia.Controls;

namespace Kubonia.Client.Navigation;

public sealed record WorkspaceTab(string Route, string Title, Control Content);

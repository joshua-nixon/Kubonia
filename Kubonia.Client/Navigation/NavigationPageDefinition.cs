using System;
using Avalonia.Controls;

namespace Kubonia.Client.Navigation;

public sealed record NavigationPageDefinition(
    string Route,
    string Title,
    Func<Control> CreateView);

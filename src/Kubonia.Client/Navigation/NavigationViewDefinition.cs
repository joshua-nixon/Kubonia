using Avalonia.Controls;

namespace Kubonia.Client.Navigation;

public interface INavigationViewDefinition
{
    string Title { get; }
    string Route { get; }

    Control CreateView(ViewFactory factory);
    
}
public sealed record NavigationViewDefinition<T>(string Title) : INavigationViewDefinition
    where T : Control
{
    public string Route { get; } = Title.ToLowerInvariant();

    public Control CreateView(ViewFactory factory) => factory.GetView<T>();
}

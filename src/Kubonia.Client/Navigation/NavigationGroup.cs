namespace Kubonia.Client.Navigation;

public sealed class NavigationGroup
{
    private readonly IReadOnlyDictionary<string, INavigationViewDefinition> _pagesByRoute;

    public NavigationGroup(
        string key,
        string title,
        IEnumerable<INavigationViewDefinition> pages)
    {


        Key = key;
        Title = title;
        Pages = pages.ToList().AsReadOnly();
        _pagesByRoute = Pages.ToDictionary(page => page.Route, StringComparer.Ordinal);
    }

    public string Key { get; }

    public string Title { get; }

    public IReadOnlyList<INavigationViewDefinition> Pages { get; }

    public bool IsExpanded { get; private set; } = true;

    public void ToggleExpansion() => IsExpanded = !IsExpanded;

    public INavigationViewDefinition GetPage(string route)
    {
        if (!_pagesByRoute.TryGetValue(route, out var page))
        {
            throw new KeyNotFoundException($"Navigation page '{route}' is not in group '{Key}'.");
        }

        return page;
    }
}

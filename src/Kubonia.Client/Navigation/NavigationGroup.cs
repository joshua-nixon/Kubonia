using System;
using System.Collections.Generic;
using System.Linq;

namespace Kubonia.Client.Navigation;

public sealed class NavigationGroup
{
    private readonly IReadOnlyDictionary<string, NavigationViewDefinition> _pagesByRoute;

    public NavigationGroup(
        string key,
        string title,
        IEnumerable<NavigationViewDefinition> pages)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(pages);

        var pageList = pages.ToArray();
        if (pageList.Length == 0)
        {
            throw new ArgumentException("A navigation group must contain at least one page.", nameof(pages));
        }

        var routes = new HashSet<string>(StringComparer.Ordinal);
        foreach (var page in pageList)
        {
            if (!routes.Add(page.Route))
            {
                throw new ArgumentException($"Duplicate navigation route '{page.Route}'.", nameof(pages));
            }
        }

        Key = key;
        Title = title;
        Pages = Array.AsReadOnly(pageList);
        _pagesByRoute = pageList.ToDictionary(page => page.Route, StringComparer.Ordinal);
    }

    public string Key { get; }

    public string Title { get; }

    public IReadOnlyList<NavigationViewDefinition> Pages { get; }

    public bool IsExpanded { get; private set; } = true;

    public void ToggleExpansion() => IsExpanded = !IsExpanded;

    public NavigationViewDefinition GetPage(string route)
    {
        if (!_pagesByRoute.TryGetValue(route, out var page))
        {
            throw new KeyNotFoundException($"Navigation page '{route}' is not in group '{Key}'.");
        }

        return page;
    }
}

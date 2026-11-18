using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Kubonia.Client.Navigation;

public sealed class WorkspaceTabCollection
{
    private readonly ObservableCollection<WorkspaceTab> _tabs = [];
    private readonly Dictionary<string, WorkspaceTab> _tabsByRoute = new(StringComparer.Ordinal);

    public WorkspaceTabCollection()
    {
        Tabs = new ReadOnlyObservableCollection<WorkspaceTab>(_tabs);
    }

    public ReadOnlyObservableCollection<WorkspaceTab> Tabs { get; }

    public WorkspaceTab GetOrAdd(NavigationViewDefinition page)
    {
        ArgumentNullException.ThrowIfNull(page);

        if (_tabsByRoute.TryGetValue(page.Route, out var existingTab))
        {
            return existingTab;
        }

        var tab = new WorkspaceTab(page.Route, page.Title, page.CreateView());
        _tabsByRoute.Add(tab.Route, tab);
        _tabs.Add(tab);
        return tab;
    }

    public WorkspaceTab? Remove(string route)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route);

        if (!_tabsByRoute.Remove(route, out var tab))
        {
            return null;
        }

        _tabs.Remove(tab);

        return tab;
    }

    public WorkspaceTab? Pop()
    {
        if (_tabs.Count == 0)
        {
            return null;
        }

        return Remove(_tabs[^1].Route);
    }
}

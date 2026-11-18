using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Kubonia.Client.Navigation;
using Kubonia.Client.Views;

namespace Kubonia.Client;

public partial class MainWindow : Window
{
    private readonly IReadOnlyDictionary<string, NavigationPageDefinition> _navigationPages;
    private readonly IReadOnlyDictionary<string, Button> _navigationButtons;
    private readonly Dictionary<string, TabItem> _openTabs = new(StringComparer.Ordinal);
    private bool _navigationReady;

    public MainWindow()
    {
        InitializeComponent();

        var navigationPages = new[]
        {
            new NavigationPageDefinition(
                "overview",
                "Overview",
                () => OverviewTab.Content as Control
                    ?? throw new InvalidOperationException("The Overview tab has no page content.")),
            new NavigationPageDefinition(
                "pods",
                "Pods",
                () => new PodsView())
        };

        _navigationPages = navigationPages.ToDictionary(
            page => page.Route,
            StringComparer.Ordinal);

        _navigationButtons = new Dictionary<string, Button>(StringComparer.Ordinal)
        {
            ["overview"] = OverviewNavigation,
            ["pods"] = PodsNavigation
        };

        _openTabs.Add("overview", OverviewTab);
        _navigationReady = true;
    }

    private void WorkloadsToggle_Click(object? sender, RoutedEventArgs e)
    {
        WorkloadsItems.IsVisible = !WorkloadsItems.IsVisible;
        WorkloadsChevron.RenderTransform = new RotateTransform(WorkloadsItems.IsVisible ? 0 : -90);
    }

    private async void NavigationButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string routeKey })
        {
            throw new InvalidOperationException("Navigation buttons must specify a route key.");
        }

        await NavigateToRouteAsync(routeKey);
    }

    private async Task NavigateToRouteAsync(string routeKey)
    {
        if (!_navigationPages.TryGetValue(routeKey, out var page))
        {
            throw new InvalidOperationException($"Navigation page '{routeKey}' is not registered.");
        }

        if (!_openTabs.TryGetValue(routeKey, out var tab))
        {
            tab = new TabItem
            {
                Header = page.Title,
                Tag = page.Route,
                Content = page.CreateView()
            };

            _openTabs.Add(routeKey, tab);

            PageTabs.Items.Add(tab);
        }

        var wasAlreadySelected = ReferenceEquals(PageTabs.SelectedItem, tab);

        PageTabs.SelectedItem = tab;

        SetSelectedNavigation(routeKey);

        if (wasAlreadySelected)
        {
            await ActivatePageAsync(tab);
        }
    }

    private async void PageTabs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_navigationReady || !ReferenceEquals(e.Source, PageTabs))
        {
            return;
        }

        if (PageTabs.SelectedItem is not TabItem tab || tab.Tag is not string routeKey)
        {
            return;
        }

        if (!_navigationPages.ContainsKey(routeKey))
        {
            throw new InvalidOperationException($"Navigation page '{routeKey}' is not registered.");
        }

        SetSelectedNavigation(routeKey);

        await ActivatePageAsync(tab);
    }

    private void SetSelectedNavigation(string routeKey)
    {
        foreach (var (key, button) in _navigationButtons)
        {
            button.Classes.Set("Selected", key == routeKey);
        }
    }

    private static async Task ActivatePageAsync(TabItem tab)
    {
        if (tab.Content is INavigationAware page)
        {
            await page.OnNavigatedToAsync();
        }
    }
}
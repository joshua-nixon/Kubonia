using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Kubonia.Client.Navigation;
using Kubonia.Client.Views;

namespace Kubonia.Client;

public partial class MainWindow : Window
{
    private readonly NavigationGroup _workloadsGroup;
    private readonly WorkspaceTabCollection _openTabs;
    private bool _navigationReady;
    private bool _isSynchronizingNavigationSelection;

    public MainWindow(ViewFactory viewFactory)
    {
        InitializeComponent();

        _openTabs = new WorkspaceTabCollection(viewFactory);

        _workloadsGroup = new NavigationGroup(
            "workloads",
            "Workloads",
            [
                new NavigationViewDefinition<OverviewView>("Overview"),
                new NavigationViewDefinition<PodsView>("Pods")
            ]);

        WorkloadsTitle.Text = _workloadsGroup.Title;
        WorkloadsItems.ItemsSource = _workloadsGroup.Pages;
        PageTabs.ItemsSource = _openTabs.Tabs;

        var overviewTab = _openTabs.GetOrAdd(_workloadsGroup.GetPage("overview"));
        PageTabs.SelectedItem = overviewTab;
        SetSelectedNavigation("overview");

        _navigationReady = true;
    }

    private void NavigationGroupToggle_Click(object? sender, RoutedEventArgs e)
    {
        _workloadsGroup.ToggleExpansion();
        WorkloadsItems.IsVisible = _workloadsGroup.IsExpanded;
        WorkloadsChevron.RenderTransform = new RotateTransform(_workloadsGroup.IsExpanded ? 0 : -90);
    }

    private async void NavigationPages_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_navigationReady ||
            _isSynchronizingNavigationSelection ||
            !ReferenceEquals(e.Source, WorkloadsItems) ||
            WorkloadsItems.SelectedItem is not INavigationViewDefinition page)
        {
            return;
        }

        await NavigateToRouteAsync(page.Route);
    }

    private async Task NavigateToRouteAsync(string routeKey)
    {
        var page = _workloadsGroup.GetPage(routeKey);
        var tab = _openTabs.GetOrAdd(page);

        var wasAlreadySelected = ReferenceEquals(PageTabs.SelectedItem, tab);

        PageTabs.SelectedItem = tab;

        SetSelectedNavigation(routeKey);

        if (wasAlreadySelected)
        {
            await ActivatePageAsync(tab);
        }
    }

    private void CloseTab_Click(object? sender, RoutedEventArgs e)
    {
        if ((sender as Control)?.DataContext is not WorkspaceTab tab)
        {
            return;
        }

        e.Handled = true;

        var wasSelected = ReferenceEquals(PageTabs.SelectedItem, tab);
        var closedIndex = _openTabs.Tabs.IndexOf(tab);
        if (_openTabs.Remove(tab.Route) is null || !wasSelected)
        {
            return;
        }

        var nextIndex = Math.Min(closedIndex, _openTabs.Tabs.Count - 1);
        var nextTab = nextIndex >= 0 ? _openTabs.Tabs[nextIndex] : null;
        PageTabs.SelectedItem = nextTab;

        if (nextTab is null)
        {
            ClearSelectedNavigation();
        }
        else
        {
            SetSelectedNavigation(nextTab.Route);
        }
    }

    private async void PageTabs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_navigationReady || !ReferenceEquals(e.Source, PageTabs))
        {
            return;
        }

        if (PageTabs.SelectedItem is not WorkspaceTab tab)
        {
            return;
        }

        var page = _workloadsGroup.GetPage(tab.Route);

        SetSelectedNavigation(page);

        await ActivatePageAsync(tab);
    }

    private void SetSelectedNavigation(string routeKey)
    {
        SetSelectedNavigation(_workloadsGroup.GetPage(routeKey));
    }

    private void SetSelectedNavigation(INavigationViewDefinition page)
    {
        _isSynchronizingNavigationSelection = true;
        try
        {
            WorkloadsItems.SelectedItem = page;
        }
        finally
        {
            _isSynchronizingNavigationSelection = false;
        }
    }

    private void ClearSelectedNavigation()
    {
        _isSynchronizingNavigationSelection = true;
        try
        {
            WorkloadsItems.SelectedItem = null;
        }
        finally
        {
            _isSynchronizingNavigationSelection = false;
        }
    }

    private static async Task ActivatePageAsync(WorkspaceTab tab)
    {
        if (tab.Content is INavigationAware page)
        {
            await page.OnNavigatedToAsync();
        }
    }
}
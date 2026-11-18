using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Kubonia.Client.Navigation;
using Kubonia.Client.Services;
using Kubonia.Shared.Pods;

namespace Kubonia.Client.Views;

public partial class PodsView : UserControl, INavigationAware
{
    private readonly IKubeServerClient _kubeServerClient;
    private readonly ObservableCollection<PodRowViewModel> _pods = [];
    private bool _isLoading;

    public PodsView(IKubeServerClient kubeServerClient)
    {
        _kubeServerClient = kubeServerClient;
        InitializeComponent();
        PodsTable.ItemsSource = _pods;
    }

    public Task LoadAsync() => RefreshPodsAsync();

    public Task OnNavigatedToAsync() => LoadAsync();

    private async void RefreshButton_Click(object? sender, RoutedEventArgs e)
    {
        await RefreshPodsAsync();
    }

    private async Task RefreshPodsAsync()
    {
        if (_isLoading)
        {
            return;
        }

        _isLoading = true;
        RefreshButton.IsEnabled = false;
        LoadingText.IsVisible = true;
        EmptyText.IsVisible = false;
        ErrorText.IsVisible = false;
        PodsTable.IsVisible = false;

        try
        {
            var response = await _kubeServerClient.GetPodsAsync();
            var rows = response.Items.Select(ToPodRow).ToArray();

            _pods.Clear();
            foreach (var row in rows)
            {
                _pods.Add(row);
            }

            PodCountText.Text = $"{_pods.Count} pods";
            LoadingText.IsVisible = false;
            EmptyText.IsVisible = _pods.Count == 0;
            PodsTable.IsVisible = _pods.Count > 0;
        }
        finally
        {
            _isLoading = false;
            RefreshButton.IsEnabled = true;
        }
    }

    private static PodRowViewModel ToPodRow(PodDTO pod)
    {
        return new PodRowViewModel(
            pod.Name,
            pod.Namespace,
            pod.Phase,
            pod.NodeName,
            pod.PodIp,
            $"{pod.ReadyContainers}/{pod.ContainerCount}",
            pod.RestartCount,
            FormatAge(pod.CreatedAt),
            pod.CreatedAt
        );
    }

    private static string FormatAge(DateTimeOffset? createdAt)
    {
        if (createdAt is null)
        {
            return "-";
        }

        var age = DateTimeOffset.UtcNow - createdAt.Value;
        if (age < TimeSpan.Zero)
        {
            return "<1m";
        }

        if (age.TotalDays >= 1)
        {
            return $"{(int)age.TotalDays}d";
        }

        if (age.TotalHours >= 1)
        {
            return $"{(int)age.TotalHours}h";
        }

        if (age.TotalMinutes >= 1)
        {
            return $"{(int)age.TotalMinutes}m";
        }

        return "<1m";
    }

}

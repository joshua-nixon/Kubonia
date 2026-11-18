using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Kubonia.Client.Services;
using Kubonia.Shared.Pods;

namespace Kubonia.Client.Views;

public partial class PodsView : UserControl
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

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        await RefreshPodsAsync();
    }

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

        SetViewState(PodsViewState.Loading);

        RefreshButton.IsEnabled = false;

        try
        {
            var response = await _kubeServerClient.GetPodsAsync();
            var rows = response.Items.Select(ToPodRow).ToArray();

            _pods.Clear();

            foreach (var row in rows)
            {
                _pods.Add(row);
            }

            SetViewState(PodsViewState.Loaded);
        }
        finally
        {
            _isLoading = false;
            RefreshButton.IsEnabled = true;
        }
    }

    private void SetViewState(PodsViewState state, string? errorMessage = null)
    {
        StateText.Text = state switch
        {
            PodsViewState.Loading => "Loading pods...",
            PodsViewState.Error => errorMessage ?? "Unable to load pods.",
            PodsViewState.Loaded => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        StateText.IsVisible = state != PodsViewState.Loaded;

        StateText.Classes.Remove("Error");

        if (state == PodsViewState.Error)
        {
            StateText.Classes.Add("Error");
        }

        PodsTable.IsVisible = state == PodsViewState.Loaded;
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

    private enum PodsViewState
    {
        Loading,
        Error,
        Loaded
    }

}

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Kubonia.Client;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void WorkloadsToggle_Click(object? sender, RoutedEventArgs e)
    {
        WorkloadsItems.IsVisible = !WorkloadsItems.IsVisible;
        WorkloadsChevron.RenderTransform = new RotateTransform(WorkloadsItems.IsVisible ? 0 : -90);
    }
}
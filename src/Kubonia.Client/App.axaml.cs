using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kubonia.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Client;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();

            services.AddTransient<MainWindow>();

            services.AddClientServices();

            var provider = services.BuildServiceProvider();

            desktop.Exit += (_, _) => provider.Dispose();

            desktop.MainWindow = provider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
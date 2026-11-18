using System;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kubonia.Client.Services;
using Kubonia.Client.Views;
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

            services.AddSingleton(_ => new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:5215/")
            });

            services.AddSingleton<IKubeServerClient, KubeServerClient>();
            services.AddTransient<PodsView>();
            services.AddSingleton<Func<PodsView>>(provider => provider.GetRequiredService<PodsView>);
            services.AddTransient<MainWindow>();

            var provider = services.BuildServiceProvider();

            desktop.Exit += (_, _) => provider.Dispose();

            desktop.MainWindow = provider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
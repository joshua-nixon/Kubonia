using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Kubonia.Client.Navigation
{
    public sealed class ViewFactory(IServiceProvider provider)
    {
        public Control GetView<T>() where T : Control
        {
            return provider.GetRequiredService<T>();
        }
    }
}

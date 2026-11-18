using System.Threading.Tasks;

namespace Kubonia.Client.Navigation;

public interface INavigationAware
{
    Task OnNavigatedToAsync();
}

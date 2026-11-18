namespace Kubonia.Client.Navigation;

public interface INavigationAware
{
    Task OnNavigatedToAsync();
}

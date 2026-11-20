using Kubonia.Shared.Pods;

namespace Kubonia.Kube
{
    public interface IKubernetesService
    {
        ValueTask<List<PodDTO>> GetPodsAsync(CancellationToken cancellationToken = default);
        ValueTask<List<PodDTO>> GetPodsAsync(string namespaceName, CancellationToken cancellationToken = default);
    }
}

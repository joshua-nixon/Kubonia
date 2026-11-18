using k8s.Models;

namespace Kubonia.Server.K8s
{
    public interface IKubernetesAccessor
    {
        ValueTask<V1PodList> ListPodsAsync(CancellationToken cancellationToken);
        ValueTask<V1PodList> ListPodsAsync(string namespaceName, CancellationToken cancellationToken);
    }
}

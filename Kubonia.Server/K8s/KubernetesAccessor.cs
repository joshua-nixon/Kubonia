using k8s;
using k8s.Models;

namespace Kubonia.Server.K8s
{
    internal sealed class KubernetesAccessor(IKubernetes kubernetes) : IKubernetesAccessor
    {
        public async ValueTask<V1PodList> ListPodsAsync(CancellationToken cancellationToken)
        {
            return await kubernetes.CoreV1.ListPodForAllNamespacesAsync(cancellationToken: cancellationToken);
        }

        public async ValueTask<V1PodList> ListPodsAsync(string namespaceName, CancellationToken cancellationToken)
        {
            return await kubernetes.CoreV1.ListNamespacedPodAsync(namespaceName, cancellationToken: cancellationToken);
        }
    }
}

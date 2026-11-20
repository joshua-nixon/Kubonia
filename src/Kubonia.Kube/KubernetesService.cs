using k8s;
using Kubonia.Shared.Pods;

namespace Kubonia.Kube
{
    internal sealed class KubernetesService(IKubernetes kubernetes) : IKubernetesService
    {
        public async ValueTask<List<PodDTO>> GetPodsAsync(CancellationToken cancellationToken)
        {
            var pods = await kubernetes.CoreV1.ListPodForAllNamespacesAsync(cancellationToken: cancellationToken);
            return [.. pods.Items.Select(pod => pod.ToPodDTO())];
        }

        public async ValueTask<List<PodDTO>> GetPodsAsync(string namespaceName, CancellationToken cancellationToken)
        {
            var pods = await kubernetes.CoreV1.ListNamespacedPodAsync(namespaceName, cancellationToken: cancellationToken);
            return [.. pods.Items.Select(pod => pod.ToPodDTO())];
        }
    }
}

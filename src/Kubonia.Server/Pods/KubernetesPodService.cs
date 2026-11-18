using Kubonia.Server.K8s;
using Kubonia.Shared.Pods;

namespace Kubonia.Server.Pods;

internal sealed class KubernetesPodService(IKubernetesAccessor accessor) : IPodService
{
    public async Task<PodListResponse> ListAsync(string? namespaceName, CancellationToken cancellationToken)
    {
        var podList = string.IsNullOrWhiteSpace(namespaceName)
            ? await accessor.ListPodsAsync(cancellationToken)
            : await accessor.ListPodsAsync(namespaceName, cancellationToken);

        var pods = podList.Items
            .Select(static p => p.ToPodDTO());

        return new PodListResponse(pods);
    }
}
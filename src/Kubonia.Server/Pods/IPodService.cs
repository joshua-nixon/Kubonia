using Kubonia.Shared.Pods;

namespace Kubonia.Server.Pods;

internal interface IPodService
{
    Task<PodListResponse> ListAsync(string? namespaceName, CancellationToken cancellationToken);
}

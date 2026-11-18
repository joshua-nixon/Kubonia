using System.Threading;
using System.Threading.Tasks;
using Kubonia.Shared.Pods;

namespace Kubonia.Client.Services;

public interface IKubeServerClient
{
    ValueTask<PodListResponse> GetPodsAsync(
        string? namespaceName = null,
        CancellationToken cancellationToken = default);
}

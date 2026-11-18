using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kubonia.Shared.Pods;

namespace Kubonia.Client.Services;

internal sealed class KubeServerClient(HttpClient httpClient) : IKubeServerClient
{
    public async ValueTask<PodListResponse> GetPodsAsync(
        string? namespaceName = null,
        CancellationToken cancellationToken = default)
    {
        var path = string.IsNullOrWhiteSpace(namespaceName)
            ? "pods"
            : $"pods?namespace={Uri.EscapeDataString(namespaceName.Trim())}";

        using var response = await httpClient.GetAsync(path, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PodListResponse>(cancellationToken)
            ?? throw new JsonException("The server returned an empty pod response.");
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kubonia.Shared.Contracts.Pods;

namespace Kubonia.Client.Services;

public sealed class KubeServerClient
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://127.0.0.1:5215/")
    };

    public async Task<PodListResponse> GetPodsAsync(
        string? namespaceName = null,
        CancellationToken cancellationToken = default)
    {
        var path = string.IsNullOrWhiteSpace(namespaceName)
            ? "pods"
            : $"pods?namespace={Uri.EscapeDataString(namespaceName.Trim())}";

        using var response = await HttpClient.GetAsync(path, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PodListResponse>(cancellationToken)
            ?? throw new JsonException("The server returned an empty pod response.");
    }
}

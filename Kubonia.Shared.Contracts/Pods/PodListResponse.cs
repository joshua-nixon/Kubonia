namespace Kubonia.Shared.Contracts.Pods;

public sealed record PodListResponse(
    IEnumerable<PodDTO> Items
);

namespace Kubonia.Shared.Pods;

public sealed record PodListResponse(
    IEnumerable<PodDTO> Items
);

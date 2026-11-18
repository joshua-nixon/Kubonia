using Microsoft.AspNetCore.Mvc;

namespace Kubonia.Server.Pods;

internal static class PodEndpoints
{
    public static IEndpointRouteBuilder MapPodEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pods", async (
            [FromQuery(Name = "namespace")] string? namespaceName,
            IPodService podService,
            CancellationToken cancellationToken) =>
        {
            var response = await podService.ListAsync(namespaceName, cancellationToken);

            return TypedResults.Ok(response);
        });

        return endpoints;
    }
}

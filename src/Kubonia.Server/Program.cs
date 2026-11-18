using Kubonia.Server.K8s;
using Kubonia.Server.Pods;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5215");

builder.Services.AddProblemDetails();

builder.Services.AddKubernetesAccessor();

builder.Services.AddPodServices();

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapPodEndpoints();

await app.RunAsync();
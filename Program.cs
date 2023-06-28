using Microsoft.AspNetCore.HttpLogging;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(c =>
{
    c.LoggingFields = HttpLoggingFields.RequestBody;
    c.RequestBodyLogLimit = 100000000;
});

builder.Services
    .AddReverseProxy()
    .LoadFromMemory(
        new[]
        {
            new RouteConfig
            {
                RouteId = "r1",
                Match = new RouteMatch { Path = "/httpbin/{**catch-all}" },
                Transforms = new []
                {
                    new Dictionary<string, string>{
                        { "PathRemovePrefix", "/httpbin" }
                    }
                },
                ClusterId = "httpbin"
            },
            new RouteConfig
            {
                RouteId = "r2",
                Match = new RouteMatch { Path = "/httpstatus/{**catch-all}" },
                Transforms = new []
                {
                    new Dictionary<string, string>{
                        { "PathRemovePrefix", "/httpstatus" }
                    }
                },
                ClusterId = "httpstatus"
            }
        },
        new[]
        {
            new ClusterConfig
            {
                ClusterId = "httpbin",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "httpbin/post", new DestinationConfig { Address = "https://httpbin.org/" } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "httpstatus",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "httpstatus/post", new DestinationConfig { Address = "http://httpstat.us/" } }
                }
            }
});

var app = builder.Build();

app.UseHttpLogging();
app.MapReverseProxy();
app.MapPost("/", () => "Hello Post!");

app.Run();

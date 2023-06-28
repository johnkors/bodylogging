
# Setup

```csharp
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
    .LoadFromMemory(…) // snipped. See Program.cs
});

var app = builder.Build();

app.UseHttpLogging();
app.MapReverseProxy();
app.MapPost("/", () => "Hello Post!");

app.Run();
```


# Request

Sending a body:

<img src="https://github.com/johnkors/bodylogging/assets/206726/ddc49e59-3e60-4c93-8915-e13b2d9312da">


Logs without Request body:

<img src="https://github.com/johnkors/bodylogging/assets/206726/a2159938-7d0b-4765-bc90-cf0aab5eec8e">


# Workaround


Adding a middleware that reads the body:

<img src="https://github.com/johnkors/bodylogging/assets/206726/88c80ab4-7ed6-4cf5-b0db-7a9c3977915d">


Logs then shows the body:

<img src="https://github.com/johnkors/bodylogging/assets/206726/b5fad83c-fed0-47b5-8a3c-7c6e0110e4c1">

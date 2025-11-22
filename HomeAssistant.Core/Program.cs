using System;
using System.IO;
using HomeAssistant.Core.Services;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// gRPC service
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        // On Windows → Named Pipes (via HttpSys alternative)
        options.ListenNamedPipe("homeassistant_core_pipe");
        Console.WriteLine("Using Windows Named Pipe: homeassistant_core_pipe");
    }
    else
    {
        // On Linux/macOS → Unix Domain Socket
        var socketPath = "/tmp/homeassistant_core.sock";

        if (File.Exists(socketPath))
            File.Delete(socketPath);

        options.ListenUnixSocket(socketPath);
        Console.WriteLine("Using Unix Domain Socket: " + socketPath);
    }
});

var app = builder.Build();

app.MapGrpcService<EventInterceptorService>();
app.MapGrpcService<EntityPlatformInterceptorService>();
app.MapGrpcService<StateInterceptorService>();


app.Run();
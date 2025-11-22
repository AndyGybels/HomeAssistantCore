using System.Threading.Tasks;
using Grpc.Core;
using Hacore;
using Microsoft.Extensions.Logging;

namespace HomeAssistant.Core.Services;

public class EventInterceptorService : EventInterceptor.EventInterceptorBase
{
    private readonly ILogger<EventInterceptorService> _logger;

    public EventInterceptorService(ILogger<EventInterceptorService> logger)
    {
        _logger = logger;
    }

    public override Task<EventResponse> InterceptEvent(
        EventMessage request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "HA Event → {Type} entity={Entity} data={Json}",
            request.EventType,
            request.EntityId,
            request.JsonData
        );

        // For now always let Home Assistant handle the event
        return Task.FromResult(new EventResponse { Handled = false });
    }
}
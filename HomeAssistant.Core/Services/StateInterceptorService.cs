using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Hacore;
using Microsoft.Extensions.Logging;

namespace HomeAssistant.Core.Services;

public class StateInterceptorService : StateInterceptor.StateInterceptorBase
{
    private readonly ILogger<StateInterceptorService> _logger;

    public StateInterceptorService(ILogger<StateInterceptorService> logger)
    {
        _logger = logger;
    }

    public override Task<StateWriteResponse> InterceptStateWrite(
        StateWriteRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "STATE CHANGE → {EntityId} = {State} attrs={Attrs}",
            request.EntityId,
            request.State,
            request.AttributesJson
        );

        // For now: allow HA to handle everything normally
        return Task.FromResult(new StateWriteResponse
        {
            Handled = false
        });
    }
}
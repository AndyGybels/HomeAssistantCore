using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Hacore;
using Microsoft.Extensions.Logging;

namespace HomeAssistant.Core.Services;

public class EntityPlatformInterceptorService : EntityPlatformInterceptor.EntityPlatformInterceptorBase
{
    private readonly ILogger<EntityPlatformInterceptorService> _logger;

    public EntityPlatformInterceptorService(ILogger<EntityPlatformInterceptorService> logger)
    {
        _logger = logger;
    }

    // ---------------------------
    // PLATFORM SETUP
    // ---------------------------
    public override Task<PlatformSetupResponse> PlatformSetup(
        PlatformSetupRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "Platform setup: {Domain}.{Platform}",
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new PlatformSetupResponse { Ok = true });
    }

    public override Task<PlatformResetResponse> PlatformReset(
        PlatformResetRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "Platform reset: {Domain}.{Platform}",
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new PlatformResetResponse { Ok = true });
    }


    // ---------------------------
    // ENTITIES
    // ---------------------------
    public override Task<EntitiesAddedResponse> EntitiesAdded(
        EntitiesAddedRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("Entities added ({Count}) for platform {Domain}.{Platform}",
            request.Entities.Count,
            request.Platform.Domain,
            request.Platform.PlatformName);

        foreach (var e in request.Entities)
        {
            _logger.LogInformation(" - {Entity}  (unique: {Unique}, poll: {ShouldPoll})",
                e.Name, e.UniqueId, e.ShouldPoll);
        }

        return Task.FromResult(new EntitiesAddedResponse { Ok = true });
    }

    public override Task<EntityAddedResponse> EntityAdded(
        EntityAddedRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "Entity added: {Id} ({Name}) to {Domain}.{Platform}",
            request.Entity.EntityId,
            request.Entity.Name,
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new EntityAddedResponse { Ok = true });
    }

    public override Task<EntityRemovedResponse> EntityRemoved(
        EntityRemovedRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "Entity removed: {Id} from {Domain}.{Platform}",
            request.EntityId,
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new EntityRemovedResponse { Ok = true });
    }


    // ---------------------------
    // UPDATE LOOP
    // ---------------------------
    public override Task<EntityUpdateTickResponse> EntityUpdateTick(
        EntityUpdateTickRequest request,
        ServerCallContext context)
    {
        _logger.LogDebug(
            "Update tick: {Domain}.{Platform}",
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new EntityUpdateTickResponse { Ok = true });
    }


    // ---------------------------
    // SERVICES
    // ---------------------------
    public override Task<RegisterEntityServiceResponse> RegisterEntityService(
        RegisterEntityServiceRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "Entity service registered: {Service} @ {Domain}.{Platform}",
            request.ServiceName,
            request.Platform.Domain,
            request.Platform.PlatformName);

        return Task.FromResult(new RegisterEntityServiceResponse { Ok = true });
    }

    public override Task<ExtractEntitiesResponse> ExtractEntities(
        ExtractEntitiesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "ExtractEntities: {Service}.{Domain} on platform {Platform}",
            request.ServiceCall.Service,
            request.ServiceCall.Domain,
            request.Platform.PlatformName);

        // Later you can do actual extraction logic
        return Task.FromResult(new ExtractEntitiesResponse
        {
            Ok = true,
        });
    }
}

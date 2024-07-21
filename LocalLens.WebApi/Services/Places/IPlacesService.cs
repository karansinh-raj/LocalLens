using LocalLens.WebApi.Contracts.Places;

namespace LocalLens.WebApi.Services.Places;

public interface IPlacesService
{
    Task<List<PlaceResponse>> GetChatResponseAsync(
        Guid userId,
        CancellationToken ct);
}

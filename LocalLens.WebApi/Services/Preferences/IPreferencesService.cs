using LocalLens.WebApi.Contracts.Preferences;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.Preferences;

public interface IPreferencesService
{
	Task<ResultT<IEnumerable<PreferenceResponse>>> GetAllPreferencesAsync(CancellationToken ct);

}

using AutoMapper;
using LocalLens.WebApi.Contracts.Preferences;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Messages.Preferences;
using LocalLens.WebApi.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace LocalLens.WebApi.Services.Preferences;

public class PreferenceService(IMapper _mapper, LocalLensDbContext _dbContext) : IPreferencesService
{

	public async Task<ResultT<IEnumerable<PreferenceResponse>>> GetAllPreferencesAsync(CancellationToken ct)
	{
		var preferences = await
			_dbContext.
			Preferences
			.ToListAsync();

		var PreferenceResponse = _mapper.Map<IEnumerable<PreferenceResponse>>(preferences);
		return (PreferenceResponse, PreferenceResponseMessages.PreferencesFetchSuccess);
	}

}

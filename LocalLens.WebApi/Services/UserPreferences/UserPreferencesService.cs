using LocalLens.WebApi.Contracts.UserPreferences;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Errors.UserPreferences;
using LocalLens.WebApi.Messages.UserPreferences;
using LocalLens.WebApi.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace LocalLens.WebApi.Services.UserPreferences;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly LocalLensDbContext _dbContext;

    public UserPreferencesService(
        LocalLensDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<string>> CreateUserPreferencesAsync(
        CreateUserPreferecesRequest request, 
        Guid userId, 
        CancellationToken ct)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) 
        {
            return UserPreferencesErrors.UserNotFound;
        }

        var preferences = await
            _dbContext
            .Preferences
            .Where(x => request.Preferences.Contains(x.Id))
            .ToListAsync();

        var existingUserPreferences = await 
            _dbContext
            .UserPreferences
            .Where(up => up.UserId == userId).ToListAsync();

        foreach (var userPreference in existingUserPreferences)
        {
            userPreference.IsDeleted = true;
        }

        _dbContext.UpdateRange(existingUserPreferences);

        var userPreferences = preferences
           // .Where(p => !existingUserPreferences.Any(eup => eup.PreferenceId == p.Id))
            .Select(p => new UserPreference
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = userId,
                Preference = p,
                PreferenceId = p.Id,
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false
            }).ToList();

        await _dbContext.UserPreferences.AddRangeAsync(userPreferences);
        var resultOfInsert = await _dbContext.SaveChangesAsync();

        if (resultOfInsert > 0)
        {
            return (UserPreferencesMessages.UserPreferencesCreated, UserPreferencesMessages.UserPreferencesCreated);
        }
        return UserPreferencesErrors.UserPreferencesCreateFailure;
    }
}

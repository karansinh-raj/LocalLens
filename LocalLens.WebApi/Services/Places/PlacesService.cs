using LocalLens.WebApi.Contracts.Places;
using LocalLens.WebApi.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI_API;

namespace LocalLens.WebApi.Services.Places;

public class PlacesService : IPlacesService
{
    private readonly OpenAIAPI _api;
    private readonly LocalLensDbContext _dbContext;

    public PlacesService(
        OpenAIAPI api,
        LocalLensDbContext dbContext)
    {
        _api = api;
        _dbContext = dbContext;
    }

    public async Task<List<PlaceResponse>> GetChatResponseAsync(
        Guid userId, 
        CancellationToken ct)
    {
        var preferences = await _dbContext
            .UserPreferences
            .Include(x => x.Preference)
            .Where(x => x.UserId == userId)
            .Select(x => x.Preference.PlaceName)
            .ToListAsync();

        var preferencesString = string.Join(", ", preferences);

        string prompt = $"""
        I am creating an AI-driven recommendation app that provides personalized suggestions for locations based on user preferences. 
        For the area of 'Ahmedabad', I am specifically looking for business-related places such as hotels, PG accommodations, hospitals, and restaurants.
        Please provide a list of locations in JSON format. Each location should include the following details:
        Location name
        Opening hours
        Place name
        Latitude
        Longitude
        Facilities (list)
        About place (short description)
        Feature list
        Reason (why this place is suggested, based on Google reviews)
        Here is the list of user preferences: {preferencesString}
        Please make sure that location name, place name and Latitude, Longitude should not be null. 
        Please ensure the suggestions align with the user's interest in business-related places. 
        Provide at least 7 items if possible.
        Please respond with atleast 7 items if possible
        """;

        var conversation = _api.Chat.CreateConversation();
        conversation.AppendUserInput(prompt);
        var response = await conversation.GetResponseFromChatbotAsync();

        var result = ParseResponseToPlaces(response);
        return result;
    }

    private static List<PlaceResponse> ParseResponseToPlaces(string response)
    {
        try
        {
            if (string.IsNullOrEmpty(response))
            {
                return [];
            }
            return JsonConvert.DeserializeObject<List<PlaceResponse>>(response) ?? [];
        }
        catch (Exception ex)
        {
            return [];
        }
    }
}

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

        var questions = await _dbContext
            .UserQuestions
            .Include(x => x.Question)
            .Include(x => x.Option)
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                QuestionText = x.Question.Text,
                OptionText = x.Option.Text
            })
        .ToListAsync();

        var preferencesString = string.Join(", ", preferences);
        var questionsString = string.Join(", ", questions.Select(q => $"{q.QuestionText}: {q.OptionText}"));

        string prompt = $"""
        I am creating an AI-driven recommendation app that provides personalized suggestions for locations based on user preferences.
        Please select the area of 'Ahemdabad' for searching locations.
        Please provide a list of locations in JSON format, where each location includes the following details:
        location name
        opening hours (e.g., ""8 AM to 7 PM"")
        place name
        latitude
        longitude
        facilities (list)
        about place (short description)
        feature list
        reason (why this place is suggested, based on Google reviews)
        Below is a list of user preferences: {preferencesString}
        Below is the list of questions and answers for the other preferences: {questionsString}
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

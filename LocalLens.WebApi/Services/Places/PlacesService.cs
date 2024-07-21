using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;

namespace LocalLens.WebApi.Services.Places;

public class PlacesService : IPlacesService
{
    private readonly OpenAIAPI _api;

    public PlacesService(OpenAIAPI api)
    {
        _api = api;
    }

    public async Task<string?> GetChatResponseAsync()
    {
        string prompt = @"""
        I am suggesting places to users based on there preferences and questions answer.

        preferences are:
        hospital, physiotherapist, movie_theater

        Questions are: 

        ""How important is it for you to live near a hospital or medical center?"" : User answer => ""Very Important""
        ""Do you have specific cuisine preferences for nearby dining options?"" : User answer => """"Yes""

        Give me response in the format of List of objects represented below 
        [
            {
                ""type"": """",
                ""name"": """",
                ""latitude"": ,
                ""longitude"": 
            }
        ]
        Please give me atleast list of 10 items
        """;

        var conversation = _api.Chat.CreateConversation();
        conversation.AppendUserInput(prompt);
        var response = await conversation.GetResponseFromChatbot();

        return response;

        //var chatRequest = new ChatRequest
        //{
        //    Model = "gpt-3.5-turbo", // Specify the model here
        //    Messages = new List<ChatMessage>
        //    {
        //        new ChatMessage
        //        {
        //            Content = prompt
        //        }
        //    },
        //    MaxTokens = 100,
        //    Temperature = 0.7
        //};

        //var completionRequest = new CompletionRequest(prompt model: "gpt-3.5-turbo");
        //var completionResult = await _api.Chat.CreateChatCompletionAsync(chatRequest);

        //if (completionResult.Choices.Count > 0)
        //{
        //    return completionResult.Choices[0].Message.Content;
        //}
        //else
        //{
        //    return null;
        //}
    }
}

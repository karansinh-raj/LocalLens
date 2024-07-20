namespace LocalLens.WebApi.Contracts.UserQuestions;

public class CreateUserQuestionsRequest
{
    public IEnumerable<QuestionOptionRequest> QuestionsAndOptions { get; set; }
}

namespace LocalLens.WebApi.Contracts.UserQuestions;

public class QuestionOptionRequest
{
    public Guid QuestionId { get; set; }
    public Guid OptionId { get; set; }
}

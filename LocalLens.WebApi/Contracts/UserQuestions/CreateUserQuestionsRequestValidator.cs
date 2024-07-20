using FluentValidation;

namespace LocalLens.WebApi.Contracts.UserQuestions;

public class CreateUserQuestionsRequestValidator : AbstractValidator<CreateUserQuestionsRequest>
{
    public CreateUserQuestionsRequestValidator()
    {
        RuleFor(x => x.QuestionsAndOptions)
            .NotEmpty();

        RuleForEach(x => x.QuestionsAndOptions)
            .Must(x => x.QuestionId != Guid.Empty && x.OptionId != Guid.Empty);
    }
}

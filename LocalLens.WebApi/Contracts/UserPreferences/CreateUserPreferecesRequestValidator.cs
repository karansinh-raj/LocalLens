using FluentValidation;

namespace LocalLens.WebApi.Contracts.UserPreferences;

public class CreateUserPreferecesRequestValidator : AbstractValidator<CreateUserPreferecesRequest>
{
    public CreateUserPreferecesRequestValidator()
    {
        RuleFor(x => x.Preferences)
            .NotEmpty();

        RuleForEach(x => x.Preferences)
            .Must(x => x != Guid.Empty);
    }
}

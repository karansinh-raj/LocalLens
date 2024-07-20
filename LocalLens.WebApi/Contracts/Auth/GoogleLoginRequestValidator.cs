using FluentValidation;

namespace LocalLens.WebApi.Contracts.Auth;

public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequest>
{
    public GoogleLoginRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}

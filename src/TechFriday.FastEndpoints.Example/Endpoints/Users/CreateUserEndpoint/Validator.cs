using FluentValidation;

namespace FastEndpoints.Example.Endpoints.Users.CreateUserEndpoint;

public class Validator : Validator<CreateUserRequest>
{
    public Validator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Firstname is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Lastname is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long");
    }
}

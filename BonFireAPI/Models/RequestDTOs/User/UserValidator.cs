using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.User
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
           .NotEmpty().WithMessage("Username is required.")
           .MinimumLength(5).WithMessage("Username must be at least 5 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(u => u.Profile_Photo)
                .NotNull().WithMessage("Profile photo is required.");

        }
    }
}

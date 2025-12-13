using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.Director
{
    public class DirectorValidator : AbstractValidator<DirectorRequest>
    {
        public DirectorValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(d => d.Biography)
                .NotEmpty().WithMessage("Biography is required");
        }
    }
}

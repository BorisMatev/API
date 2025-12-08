using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.Director
{
    public class ActorValidator : AbstractValidator<ActorRequest>
    {
        public ActorValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(d => d.Biography)
                .NotEmpty().WithMessage("Biography is required");
        }
    }
}

using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.Actor
{
    public class ActorValidator : AbstractValidator<ActorRequest>
    {
        public ActorValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(d => d.Photo)
                .NotNull().WithMessage("Photo is required");
        }
    }
}

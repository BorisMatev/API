using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.Review
{
    public class ReviewValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewValidator()
        {
            RuleFor(r => r.Comment)
                .NotEmpty().WithMessage("Comment required");

            RuleFor(r => r.Rating)
                .GreaterThan(0).WithMessage("Rating should be more than 0")
                .LessThanOrEqualTo(10).WithMessage("Invalid rating");
        }
    }
}

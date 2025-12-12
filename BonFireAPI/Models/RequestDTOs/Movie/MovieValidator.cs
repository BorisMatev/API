using FluentValidation;

namespace BonFireAPI.Models.RequestDTOs.Movie
{
    public class MovieValidator: AbstractValidator<MovieRequest>
    {
        public MovieValidator() 
        {
            RuleFor(m => m.Photo)
                .NotNull().WithMessage("Photo is required");

            RuleFor(m => m.Title)
                .NotEmpty().WithMessage("Title is required");

            RuleFor(m => m.DirectorName)
                .NotEmpty().WithMessage("Director is required");

            RuleFor(m => m.Release_Date)
                .NotEmpty().WithMessage("Date is required");
        }
    }
}

using BonFireAPI.Models.RequestDTOs;
using BonFireAPI.Models.RequestDTOs.Review;
using BonFireAPI.Models.ResponseDTOs;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : BaseController<Review,ReviewService,ReviewRequest,ReviewResponse>
    {
        ReviewService service = new ReviewService();
        protected override void PopulateEntity(Review forUpdate, ReviewRequest model, out string error)
        { 
            error = null;

            var userId = int.Parse(User.FindFirst("loggedUserId")?.Value!);

            var review = service.GetAll()
                .FirstOrDefault(x => 
                    x.UserId == userId && 
                    x.MovieId == model.MovieId
                );

            if (review != null)
            {
                error = "Review already added!";
            }

            forUpdate.MovieId = model.MovieId;
            forUpdate.UserId = userId;
            forUpdate.Rating = model.Rating;
            forUpdate.Comment = model.Comment;
        }

        protected override void PopulateResponseEntity(ReviewResponse forUpdate, Review model, out string error)
        { 
            error = null;

            forUpdate.Id = model.Id;
            forUpdate.Rating = model.Rating;
            forUpdate.Comment = model.Comment;
        }

        [Authorize(Roles = ["User", "Admin"])]
        public override IActionResult GetAll() => base.GetAll();
    }
}

using BonFireAPI.Models.RequestDTOs;
using BonFireAPI.Models.RequestDTOs.Review;
using BonFireAPI.Models.ResponseDTOs;
using BonFireAPI.Models.ResponseDTOs.Review;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BonFireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : BaseController<Review, ReviewService, ReviewRequest, ReviewResponse, ReviewGetRequest, ReviewGetResponse>
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
            forUpdate.UserName = model.User.Username;
        }

        protected virtual Expression<Func<Review, bool>> GetFilter(ReviewGetRequest model)
        {
            model.Filter ??= new ReviewGetFilterRequest();

            return
                u => (model.Filter.MovieId == null || u.MovieId == model.Filter.MovieId) &&
                     (model.Filter.Rating == null || u.Rating == model.Filter.Rating);
        }

        protected virtual void PopulateGetResponse(ReviewGetRequest request, ReviewGetResponse response)
        {
            response.Filter = request.Filter;
        }
    }
}

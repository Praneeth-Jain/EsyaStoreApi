using System.Security.Claims;
using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{prodId}")]
        public async Task<IActionResult> GetAllReviews(int ProdId)
        {
            var review = await _context.reviews.Where(r => r.ProductID == ProdId).ToListAsync();
            if (review is null)
            {
                return NotFound("No review found for this product.");
            }
            var reviewDto = new List<ReviewDetailsDTO>();
            foreach(var rev in review)
            {
            var thisrev=new ReviewDetailsDTO
            {
                ProductName =await _context.products.Where(p => p.Id == rev.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync(),
                UserName =await _context.users.Where(u=>u.Id==rev.UserID).Select(u=>u.Name).FirstOrDefaultAsync(),
                ReviewDate = rev.ReviewDate,
                ReviewDescription = rev.ReviewDescription,
                Stars=rev.Stars
            };
                reviewDto.Add(thisrev);
            }

            return Ok(reviewDto);
                
        }

        [HttpPost]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> AddReview(AddReviewDTO newReview)
        {
            var UserClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserClaim == null) { return Unauthorized(); };
            if (UserClaim != newReview.UserID.ToString()) { return Unauthorized("You are not authorized to add this review"); }
            var order=await _context.orders.Where(o=>o.UserId==newReview.UserID && o.ProductId==newReview.ProductID).FirstOrDefaultAsync();
            if (order == null) { return BadRequest("You have not ordered this product to review."); };
            var review=await _context.reviews.Where(r=>r.UserID==newReview.UserID && r.ProductID==newReview.ProductID).FirstOrDefaultAsync();
            if (review != null) { return BadRequest("You have already provided the review for this product."); };
                var addReview = new Reviews()
            {
                ProductID = newReview.ProductID,
                UserID = newReview.UserID,
                Stars = newReview.Stars,
                ReviewDate = newReview.ReviewDate,
                ReviewDescription = newReview.ReviewDescription
            };
            _context.reviews.Add(addReview);
            await _context.SaveChangesAsync();
            return Created();
        }
    }
}

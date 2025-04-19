using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.Models;
using System.Linq;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewsRepository _reviewsRepository;

        public ReviewController(ReviewsRepository reviewsRepository)
        {
            _reviewsRepository = reviewsRepository;
        }

        // GET: api/Review
        [HttpGet]
        public IActionResult GetAllReviews()
        {
            var reviews = _reviewsRepository.GetAllReviews();
            return Ok(reviews);
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public IActionResult GetReview(int id)
        {
            var review = _reviewsRepository.GetAllReviews().FirstOrDefault(r => r.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // POST: api/Review
        [HttpPost]
        public IActionResult CreateReview([FromBody] Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewsRepository.AddReview(review);
                return CreatedAtAction(nameof(GetReview), new { id = review.ReviewID }, review);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.ReviewID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _reviewsRepository.UpdateReview(review);
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _reviewsRepository.GetAllReviews().FirstOrDefault(r => r.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            _reviewsRepository.DeleteReview(id);
            return NoContent();
        }
    }
}

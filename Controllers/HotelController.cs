using SmartHotelBookingSystem.BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHotelBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using SmartHotelBookingSystem.DTOs;


namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelBLL _hotelLogic;

        public HotelController(HotelBLL hotelLogic)
        {
            _hotelLogic = hotelLogic;
        }
        [Authorize(Roles = "admin,Customer")]
        [HttpGet]
        public IActionResult GetHotels()
        {
            List<Hotel> hotels = _hotelLogic.GetAllHotels();
            return Ok(hotels);
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("{id}")]
        public IActionResult GetHotel(int id)
        {
            List<Hotel> hotels = _hotelLogic.GetAllHotels();
            Hotel hotel = hotels.Find(h => h.HotelID == id);
            if (hotel != null)
            {
                string jsonResult = JsonConvert.SerializeObject(hotel);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            var hotel = new Hotel
            {
                HotelID = hotelDTO.HotelID,
                Name = hotelDTO.Name,
                Location = hotelDTO.Location,
                ManagerID = hotelDTO.ManagerID,
                Amenities = hotelDTO.Amenities,
                Rating = hotelDTO.Rating
            };
            int result = _hotelLogic.InsertHotel(hotel);
            if (result > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotel);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the hotel.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{id}")]
        public IActionResult UpdateHotel([FromBody] UpdateHotelDTO updatehotel, int id)
        {
            var hotel = new Hotel
            {
                Name = updatehotel.Name,
                Location = updatehotel.Location,
                ManagerID = updatehotel.ManagerID,
                Amenities = updatehotel.Amenities,
                Rating = updatehotel.Rating,
                IsActive = updatehotel.IsActive
            };
            int result = _hotelLogic.UpdateHotel(hotel, id);
            if (result > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotel);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the hotel.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteHotel(int id)
        {
            var deleteStatus = _hotelLogic.DeleteHotel(id);
            if (deleteStatus != null)
            {
                List<Hotel> hotels = _hotelLogic.GetAllHotels();
                string jsonResult = JsonConvert.SerializeObject(hotels);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/amenities")]
        public IActionResult UpdateHotelAmenities(int id, [FromBody] string amenities)
        {
            int result = _hotelLogic.UpdateHotelAmenities(id, amenities);
            if (result > 0)
            {
                return Ok("Amenities updated successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the amenities.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/rating")]
        public IActionResult UpdateHotelRating(int id, [FromBody] double rating)
        {
            int result = _hotelLogic.UpdateHotelRating(id, rating);
            if (result > 0)
            {
                return Ok("Rating updated successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the rating.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("manager/{managerId}")]
        public IActionResult GetHotelsByManager(int managerId)
        {
            var hotels = _hotelLogic.ReadHotelByManagerId(managerId);
            if (hotels != null && hotels.Rows.Count > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotels);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("filter/rating")]
        public IActionResult FilterHotelsByRating([FromQuery] double minRating, [FromQuery] double maxRating)
        {
            var hotels = _hotelLogic.FilterHotelsByRating(minRating, maxRating);
            if (hotels != null && hotels.Rows.Count > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotels);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,Customer")]
        [HttpGet("filter/amenities")]
        public IActionResult FilterHotelsByAmenities([FromQuery] string amenities)
        {
            var hotels = _hotelLogic.FilterHotelsByAmenities(amenities);
            if (hotels != null && hotels.Rows.Count > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotels);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin,Customer")]
        [HttpGet("availability")]
        public IActionResult GetHotelsByAvailability([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var hotels = _hotelLogic.ReadHotelsByAvailability(startDate, endDate);
            if (hotels != null && hotels.Rows.Count > 0)
            {
                string jsonResult = JsonConvert.SerializeObject(hotels);
                return Content(jsonResult, "application/json");
            }
            else
            {
                return NotFound();
            }
        }
    }
}

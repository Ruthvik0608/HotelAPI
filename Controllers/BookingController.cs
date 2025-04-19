using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;

namespace SmartHotelBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository _repository;

        public BookingController(BookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddBooking([FromBody] Booking booking)
        {
            if (booking == null)
                return BadRequest("Invalid booking data.");

            _repository.AddBooking(booking);
            return Ok("Booking added successfully.");
        }

        [HttpGet]
        public ActionResult<List<Booking>> GetAllBookings()
        {
            var bookings = _repository.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public ActionResult<List<Booking>> GetBookingsByBookingID(int id)
        {
            var bookings = _repository.GetBookingsByBookingID(id);
            if (bookings == null || bookings.Count == 0)
                return NotFound("No booking found.");

            return Ok(bookings);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] DateTime checkInDate)
        {
            try
            {
                _repository.UpdateBooking(id, checkInDate);
                return Ok("Booking updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                _repository.DeleteBooking(id);
                return Ok("Booking deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
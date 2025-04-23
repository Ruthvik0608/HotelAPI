using SmartHotelBookingSystem.BusinessLogicLayer; // Updated namespace
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyAccountController : ControllerBase
    {
        private readonly LoyaltyDataOperations _dataOperations; // Updated class name

        public LoyaltyAccountController(LoyaltyDataOperations dataOperations) // Updated class name
        {
            _dataOperations = dataOperations;
        }

        // GET: api/LoyaltyAccount
        [HttpGet]
        public async Task<IActionResult> GetLoyaltyAccounts()
        {
            try
            {
                var accounts = await _dataOperations.GetAllLoyaltyAccountsAsync();

                if (accounts == null || accounts.Count == 0)
                {
                    return NotFound("No loyalty accounts found in the database.");
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/LoyaltyAccount/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoyaltyAccountById(int id)
        {
            try
            {
                var account = await _dataOperations.GetLoyaltyAccountByIdAsync(id);

                if (account == null)
                {
                    return NotFound($"Loyalty account with ID {id} not found.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/LoyaltyAccount
        [HttpPost]
        public async Task<IActionResult> CreateLoyaltyAccount([FromBody] LoyaltyAccount newAccount)
        {
            try
            {
                await _dataOperations.AddLoyaltyAccountAsync(newAccount);
                return Ok("Loyalty account created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/LoyaltyAccount/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateLoyaltyAccountByUserId(int userId, [FromBody] UpdateLoyaltyAccountRequest request)
        {
            try
            {
                await _dataOperations.UpdateLoyaltyAccountByUserIdAsync(userId, request.NewPointsBalance, request.IsActive);
                return Ok("Loyalty account updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class UpdateLoyaltyAccountRequest
        {
            public int NewPointsBalance { get; set; }
            public bool IsActive { get; set; }
        }

        // DELETE: api/LoyaltyAccount/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyAccount(int id)
        {
            try
            {
                var result = await _dataOperations.DeleteLoyaltyAccountAsync(id);

                if (!result)
                {
                    return NotFound($"Loyalty account with ID {id} not found.");
                }

                return Ok("Loyalty account deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

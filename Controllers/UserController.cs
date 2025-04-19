using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "admin")] //by admin
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        [Authorize] //by self and user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserAccount updatedUser)
        {
            var loggedInUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdClaim))
            {
                return Unauthorized("User ID claim not found");
            }

            if (!int.TryParse(loggedInUserIdClaim, out var loggedInUserId))
            {
                return Unauthorized("Invalid User ID claim");
            }

            if (User.IsInRole("Admin") || loggedInUserId == id)
            {
                await _userRepository.UpdateUser(id, updatedUser.Name, updatedUser.Email, updatedUser.ContactNumber);
                return Ok("User updated successfully");
            }

            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetAllUsers();
            if (user == null) return NotFound("User not found");

            await _userRepository.DeleteUser(id);
            return Ok("User deleted");
        }

        [Authorize(Roles = "admin")] //only admin
        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Invalid user data");

            var users = await _userRepository.GetUsersByName(name);
            if (users == null || users.Count == 0)
                return NotFound($"No users found with name '{name}'");

            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/total-users")]
        public async Task<IActionResult> GetTotalUsers()
        {
            var totalUsers = await _userRepository.GetTotalUser();
            return Ok(new { TotalUsers = totalUsers });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/users-by-role")]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string role)
        {
            if (string.IsNullOrEmpty(role)) return BadRequest("Role is required.");

            var users = await _userRepository.GetUsersByRole(role);
            return Ok(users);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.DTOs;
using SmartHotelBookingSystem.Models;
using SmartHotelBookingSystem.Services;
using System.Threading.Tasks;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(UserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin/register")]
        public async Task<IActionResult> AdminRegister([FromBody] AdminUserRegistrationRequest newUserRequest)
        {
            if (newUserRequest == null) return BadRequest("Invalid user data.");

            var newUser = new UserAccount
            {
                Name = newUserRequest.Name,
                Email = newUserRequest.Email,
                Password = newUserRequest.Password,
                ContactNumber = newUserRequest.ContactNumber,
                Role = string.IsNullOrEmpty(newUserRequest.Role) ? "Customer" : newUserRequest.Role
            };

            await _userRepository.AddUsers(newUser);
            return Ok(new { message = $"User registered successfully as {newUser.Role}" });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest newUserRequest)
        {
            if (newUserRequest == null) return BadRequest("Invalid user data.");

            var newUser = new UserAccount
            {
                Name = newUserRequest.Name,
                Email = newUserRequest.Email,
                Password = newUserRequest.Password,
                ContactNumber = newUserRequest.ContactNumber,
                Role = "Customer"
            };

            await _userRepository.AddUsers(newUser);
            return Ok(new { message = "User registered successfully as Customer" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _userRepository.ValidateUser(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            else
            {
                var token = _jwtTokenGenerator.GenerateToken(user);
                return Ok(new { Token = token, Role = user.Role });
            }
        }
    }
}

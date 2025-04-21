using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.Models;
using System.Collections.Generic;

namespace SmartHotelBookingSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyAccountController : ControllerBase
    {
        private readonly LoyaltyDataOperations _loyaltyDataOperations;

        public LoyaltyAccountController(LoyaltyDataOperations loyaltyDataOperations)
        {
            _loyaltyDataOperations = loyaltyDataOperations;
        }

        // GET: api/LoyaltyAccount
        [HttpGet]
        public ActionResult<IEnumerable<LoyaltyAccount>> GetLoyaltyAccounts()
        {
            return _loyaltyDataOperations.GetAllLoyaltyAccounts();
        }

        // GET: api/LoyaltyAccount/inactive
        [HttpGet("inactive")]
        public ActionResult<IEnumerable<LoyaltyAccount>> GetInactiveLoyaltyAccounts()
        {
            return _loyaltyDataOperations.GetInactiveLoyaltyAccounts();
        }

        // GET: api/LoyaltyAccount/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<LoyaltyAccount>> GetLoyaltyAccountsByUserId(int userId)
        {
            return _loyaltyDataOperations.GetLoyaltyAccountsByUserId(userId);
        }

        // GET: api/LoyaltyAccount/{id}
        [HttpGet("{id}")]
        public ActionResult<LoyaltyAccount> GetLoyaltyAccount(int id)
        {
            var loyaltyAccount = _loyaltyDataOperations.GetLoyaltyAccountById(id);

            if (loyaltyAccount == null)
            {
                return NotFound();
            }

            return loyaltyAccount;
        }

        // POST: api/LoyaltyAccount
        [HttpPost]
        public ActionResult<LoyaltyAccount> PostLoyaltyAccount(LoyaltyAccount loyaltyAccount)
        {
            _loyaltyDataOperations.AddLoyaltyAccount(loyaltyAccount);
            return CreatedAtAction(nameof(GetLoyaltyAccount), new { id = loyaltyAccount.LoyaltyID }, loyaltyAccount);
        }

        // PUT: api/LoyaltyAccount/{id}
        [HttpPut("{id}")]
        public IActionResult PutLoyaltyAccount(int id, LoyaltyAccount loyaltyAccount)
        {
            if (id != loyaltyAccount.LoyaltyID)
            {
                return BadRequest();
            }

            _loyaltyDataOperations.UpdateLoyaltyAccountByUserId(loyaltyAccount.UserID, loyaltyAccount.PointsBalance, loyaltyAccount.IsActive);
            return NoContent();
        }

        // PUT: api/LoyaltyAccount/activate/{id}
        [HttpPut("activate/{id}")]
        public IActionResult ActivateLoyaltyAccount(int id)
        {
            var result = _loyaltyDataOperations.ActivateLoyaltyAccount(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/LoyaltyAccount/addpoints/{id}
        [HttpPut("addpoints/{id}")]
        public IActionResult AddPointsToLoyaltyAccount(int id, [FromBody] int points)
        {
            _loyaltyDataOperations.AddPointsToLoyaltyAccount(id, points);
            return NoContent();
        }

        // PUT: api/LoyaltyAccount/redeempoints/{id}
        [HttpPut("redeempoints/{id}")]
        public IActionResult RedeemPointsFromLoyaltyAccount(int id, [FromBody] int points)
        {
            var result = _loyaltyDataOperations.RedeemPointsFromLoyaltyAccount(id, points);
            if (!result)
            {
                return BadRequest("Insufficient points or account not found.");
            }

            return NoContent();
        }

        // DELETE: api/LoyaltyAccount/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteLoyaltyAccount(int id)
        {
            var result = _loyaltyDataOperations.DeleteLoyaltyAccount(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/LoyaltyAccount/pointsrange
        [HttpGet("pointsrange")]
        public ActionResult<IEnumerable<LoyaltyAccount>> GetLoyaltyAccountsByPointsRange([FromQuery] int minPoints, [FromQuery] int maxPoints)
        {
            return _loyaltyDataOperations.GetLoyaltyAccountsByPointsRange(minPoints, maxPoints);
        }
    }
}

using SmartHotelBookingSystem.BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHotelBookingSystem.Models;
using System.Data;
using SmartHotelBookingSystem.DataAccess.ADO;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomBLL _roomBLL;

        public RoomController(DB1 dalObject)
        {
            _roomBLL = new RoomBLL(dalObject);
        }

        // GET: api/Room
        [HttpGet]
        public ActionResult<IEnumerable<Room_New>> GetRooms()
        {
            try
            {
                var dataTable = _roomBLL.FetchAllActiveRooms();

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return NotFound("No rooms found in the database.");
                }

                var roomList = _roomBLL.ConvertDataTableToList(dataTable);

                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Room/{id}
        [HttpGet("{id}")]
        public ActionResult<Room_New> GetRoomById(int id)
        {
            try
            {
                var dataTable = _roomBLL.FetchAllActiveRooms();

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return NotFound($"Room with ID {id} not found.");
                }

                var roomList = _roomBLL.ConvertDataTableToList(dataTable);
                var room = roomList.Find(r => r.RoomID == id);

                if (room == null)
                {
                    return NotFound($"Room with ID {id} not found.");
                }

                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Room
        [HttpPost]
        public ActionResult CreateRoom([FromBody] Room_New newRoom)
        {
            try
            {
                int result = _roomBLL.InsertRoom(newRoom);

                if (result > 0)
                {
                    return Ok("Room created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create room.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Room/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateRoom(int id, [FromBody] Room_New updatedRoom)
        {
            try
            {
                updatedRoom.RoomID = id;
                int result = _roomBLL.UpdateRoom(updatedRoom);

                if (result > 0)
                {
                    return Ok("Room updated successfully.");
                }
                else
                {
                    return NotFound($"Room with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Room/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteRoom(int id)
        {
            try
            {
                var room = new Room_New { RoomID = id };
                var dataTable = _roomBLL.DeleteRoom(room);

                if (dataTable != null)
                {
                    return Ok("Room deleted successfully.");
                }
                else
                {
                    return NotFound($"Room with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Room/Hotel/{hotelID}
        [HttpGet("Hotel/{hotelID}")]
        public ActionResult<IEnumerable<Room_New>> GetRoomsByHotel(int hotelID)
        {
            try
            {
                var dataTable = _roomBLL.FetchRoomsByHotel(hotelID);

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return NotFound($"No rooms found for Hotel ID {hotelID}.");
                }

                var roomList = _roomBLL.ConvertDataTableToList(dataTable);

                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Room/Type/{type}/Hotel/{hotelID}
        // GET: api/Room/Type/{type}/Hotel/{hotelID}
        // GET: api/Room/Type/{type}/Hotel/{hotelID}
        // GET: api/Room/Type/{type}/Hotel/{hotelID}
        [HttpGet("Type/{type}/Hotel/{hotelID}")]
        public ActionResult<IEnumerable<Room_New>> GetRoomsByTypeAndLocation(string type, int hotelID)
        {
            try
            {
                var dataTable = _roomBLL.FetchRoomsByTypeAndLocation(type, hotelID);

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return NotFound($"No rooms of type {type} found for Hotel ID {hotelID}.");
                }

                var roomList = _roomBLL.ConvertDataTableToList(dataTable);

                return Ok(roomList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}

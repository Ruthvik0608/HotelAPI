using Microsoft.AspNetCore.Mvc;
using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using SmartHotelBookingSystem.BusinessLogicLayer;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDAL _categoryDAL;

        public CategoryController(CategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryModel>> GetAllCategories()
        {
            var categories = _categoryDAL.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{name}")]
        public ActionResult<IEnumerable<CategoryModel>> GetCategoriesByName(string name)
        {
            var categories = _categoryDAL.GetCategoriesByName(name);
            return Ok(categories);
        }

        [HttpPost]
        public ActionResult AddCategory([FromBody] CategoryModel newCategory)
        {
            _categoryDAL.AddCategory(newCategory);
            return CreatedAtAction(nameof(GetCategoriesByName), new { name = newCategory.Category }, newCategory);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCategory(int id, [FromBody] CategoryModel updatedCategory)
        {
            _categoryDAL.UpdateCategory(id, updatedCategory.Category, updatedCategory.CatActivateDate, updatedCategory.CatDeactivateDate, updatedCategory.Remarks);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            _categoryDAL.DeleteCategory(id);
            return NoContent();
        }
    }
}

using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericCategoriesController : ControllerBase
    {
        public readonly IGenericRepository<Category> _category;
        public GenericCategoriesController(IGenericRepository<Category> category)
        {
            _category = category;
        }
        [HttpGet]
        public async Task <IActionResult> GetAll() {
            try
            {
                var categories = await _category.GetAllAsync();
                if (categories is null || !categories.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No categories found",
                        Data = new List<Category>()
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Categories retrieved successfully",
                    Data = categories
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving categories",
                    Error = ex.Message
                });

            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            try
            {
                var category = await _category.GetByIdAsync(id);
                if (category is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with ID {id} not found",
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category retrieved successfully",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving the category",
                    Error = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTo category)
        {
            try
            {
                var Category = new Category
                {
                    Name = category.Name,
                };
                await _category.CreateAsync(Category);
                await _category.SaveAsync();
                return StatusCode(201, new
                {
                    Message = "Category created successfully",
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving the category",
                    Error = ex.Message
                });
            }
        }
    }
}

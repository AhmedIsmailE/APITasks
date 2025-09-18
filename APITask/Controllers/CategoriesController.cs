using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // DI
        public readonly ICategoryServices _categoryServices;
        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryServices.GetAllAsync();
            try
            {
                if (categories == null || !categories.Any())
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
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving categories",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            try
            {
                if (category == null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with {id} not found",
                        Data = new Category()
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
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving category",
                    Error = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTo categoryDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid category data",
                        Errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage)
                    });
                }
                var Category = await _categoryServices.CreateAsync(categoryDTo);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Category created successfully",
                    Data = Category,
                    StatusCode = StatusCodes.Status201Created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating category",
                    Error = ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryDTo categoryDTo)
        {
            try
            {
                var existingCategory = await _categoryServices.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {id} not found",
                    });
                }
                if (await _categoryServices.UpdateAsync(id, categoryDTo))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category updated successfully",
                        Data = await _categoryServices.GetByIdAsync(id)
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Failed to update category"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while updating category",
                    Error = ex.Message
                });
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edit(CategoryDTo categoryDTo)
        {
            try
            {
                var existingCategory = await _categoryServices.GetByIdAsync(categoryDTo.Id);
                if (existingCategory == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {categoryDTo.Id} not found",
                    });
                }
                if (await _categoryServices.UpdateAsync(categoryDTo))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category updated successfully",
                        Data = await _categoryServices.GetByIdAsync(categoryDTo.Id)
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Failed to update category"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while updating category",
                    Error = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingCategory = await _categoryServices.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {id} not found",
                    });
                }
                if (await _categoryServices.DeleteAsync(id))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Data = existingCategory,
                        Message = "Category deleted successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Failed to delete category"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while deleting category",
                    Error = ex.Message
                });
            }
        }
    }
}

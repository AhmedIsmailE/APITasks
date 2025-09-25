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
        //public readonly ICategoryServices _categoryServices;
        //public CategoriesController(ICategoryServices categoryServices)
        //{
        //    _categoryServices = categoryServices;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.CategoryServices.GetAllAsync();
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
                    Data = categories.Select(category => new
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Posts = category.Posts.Select(post => new{ 
                            PostId = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreatedAt = post.CreatedAt,
                            UserId = post.UserId,
                            UserName = post.User.UserName
                        })
                    })
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
            var category = await _unitOfWork.CategoryServices.GetByIdAsync(id);
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
                    Data = new
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Posts = category.Posts.Select(post => new {
                            PostId = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreatedAt = post.CreatedAt,
                            UserId = post.UserId,
                            UserName = post.User.UserName
                        })
                    }
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
                var Category = await _unitOfWork.CategoryServices.CreateAsync(categoryDTo);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Category created successfully",
                    Data = new
                    {
                        Id = Category.Id,
                        Name = Category.Name,
                        Posts = Category.Posts.Select(post => new {
                            PostId = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreatedAt = post.CreatedAt,
                            UserId = post.UserId,
                            UserName = post.User.UserName
                        })
                    },
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
                var existingCategory = await _unitOfWork.CategoryServices.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {id} not found",
                    });
                }
                if (await _unitOfWork.CategoryServices.UpdateAsync(id, categoryDTo))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category updated successfully",
                        Data = await _unitOfWork.CategoryServices.GetByIdAsync(id)
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
                var existingCategory = await _unitOfWork.CategoryServices.GetByIdAsync(categoryDTo.Id);
                if (existingCategory == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {categoryDTo.Id} not found",
                    });
                }
                if (await _unitOfWork.CategoryServices.UpdateAsync(categoryDTo))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category updated successfully",
                        Data = await _unitOfWork.CategoryServices.GetByIdAsync(categoryDTo.Id)
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
                var existingCategory = await _unitOfWork.CategoryServices.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {id} not found",
                    });
                }
                if (await _unitOfWork.CategoryServices.DeleteAsync(id))
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Data =new
                        {
                            Id = existingCategory.Id,
                            Name = existingCategory.Name,
                            Posts = existingCategory.Posts.Select(post => new {
                                PostId = post.Id,
                                Title = post.Title,
                                Content = post.Content,
                                CreatedAt = post.CreatedAt,
                                UserId = post.UserId,
                                UserName = post.User.UserName
                            })
                        },
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

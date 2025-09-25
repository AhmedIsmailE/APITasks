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
        //public readonly IGenericRepository<Category> _category;
        //public GenericCategoriesController(IGenericRepository<Category> category)
        //{
        //    _category = category;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public GenericCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task <IActionResult> GetAll() {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
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
                    Data = categories.Select(category => new 
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Posts = category.Posts.Select(post => new
                        {
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
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
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
                    Data = new
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Posts = category.Posts.Select(post => new
                        {
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
                await _unitOfWork.Categories.CreateAsync(Category);
                await _unitOfWork.SaveAsync();
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

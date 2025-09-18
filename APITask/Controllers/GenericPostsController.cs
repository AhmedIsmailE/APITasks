using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericPostsController : ControllerBase
    {
        // DI
        private readonly IGenericRepository<Post> _post;
        public GenericPostsController(IGenericRepository<Post> post)
        {
            _post = post;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var posts = await _post.GetAllAsync();
                if (posts is null || !posts.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No posts found",
                        Data = new List<Post>()
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Posts retrieved successfully",
                    Data = posts
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving posts",
                    Error = ex.Message
                });

            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var post = await _post.GetByIdAsync(id);
                if (post is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Post with ID {id} not found",
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Post retrieved successfully",
                    Data = post
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving the post",
                    Error = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(PostDTo post)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid post data",
                        Errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage)
                    });
                }
                var Post = new Post
                {
                    CategoryId = post.CategoryId,
                    Title = post.Title,
                    CreatedAt = DateTime.Now,
                    Content = post.Content,
                    UserId = post.UserId,
                };
                await _post.CreateAsync(Post);
                await _post.SaveAsync();
                return Ok(new
                {
                    Messege = "Post created successfully",
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the post",
                    Error = ex.Message
                });
            }
                
        }
        [HttpPut]
        public async Task<IActionResult> Update(PostDTo postDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid post data",
                        Errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage)
                    });
                }
                var oldPost = await _post.GetByIdAsync(postDTo.UserId);
                if (oldPost == null)
                {
                    return NotFound(new
                    {
                        Message = "Post not found",
                        StatusCode= 404,
                    });
                }
                oldPost.CategoryId = postDTo.CategoryId;
                oldPost.Content = postDTo.Content;
                oldPost.Title = postDTo.Title;
                _post.Update(oldPost);
                await _post.SaveAsync();
                return Ok(new
                {
                    Messege = "Post Updated successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the post",
                    Error = ex.Message
                });
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var post = await _post.GetByIdAsync(id);
                if (post is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Post with ID {id} not found",
                    });
                }
                _post.Delete(post);
                await _post.SaveAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Post deleted successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while deleting the post",
                    Error = ex.Message
                });
            }
        }
    }
}

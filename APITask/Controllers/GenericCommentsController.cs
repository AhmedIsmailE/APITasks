using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericCommentsController : ControllerBase
    {
        // DI
        private readonly IGenericRepository<Comment> _comment;
        public GenericCommentsController(IGenericRepository<Comment> comment)
        {
            _comment = comment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var comments = await _comment.GetAllAsync();
                if (comments is null || !comments.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No comments found",
                        Data = new List<Post>()
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Comments retrieved successfully",
                    Data = comments
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving comments",
                    Error = ex.Message
                });

            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var comment = await _comment.GetByIdAsync(id);
                if (comment is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Comment with ID {id} not found",
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Comment retrieved successfully",
                    Data = comment
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
        public async Task<IActionResult> Add(CommentDTo commentDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid comment data",
                        Errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage)
                    });
                }
                var comment = new Comment
                {
                    PostId = commentDTo.PostId,
                    CreatedAt = DateTime.Now,
                    Content = commentDTo.Content,
                    UserId = commentDTo.UserId,
                };
                await _comment.CreateAsync(comment);
                await _comment.SaveAsync();
                return Ok(new
                {
                    Messege = "Comment created successfully",
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the comment",
                    Error = ex.Message
                });
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update(CommentDTo CommentDTo)
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
                var oldComment = await _comment.GetByIdAsync(CommentDTo.UserId);
                if (oldComment == null)
                {
                    return NotFound(new
                    {
                        Message = "Comment not found",
                        StatusCode = 404,
                    });
                }
                oldComment.Content = CommentDTo.Content;
                
                _comment.Update(oldComment);
                await _comment.SaveAsync();
                return Ok(new
                {
                    Messege = "Comment Updated successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the comment",
                    Error = ex.Message
                });
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var comment = await _comment.GetByIdAsync(id);
                if (comment is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Comment with ID {id} not found",
                    });
                }
                _comment.Delete(comment);
                await _comment.SaveAsync();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Comment deleted successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while deleting the comment",
                    Error = ex.Message
                });
            }
        }
    }
}

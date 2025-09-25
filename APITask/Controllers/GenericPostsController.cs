using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericPostsController : ControllerBase
    {
        // DI
        //private readonly IGenericRepository<Post> _post;
        //public GenericPostsController(IGenericRepository<Post> post)
        //{
        //    _post = post;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public GenericPostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetAllAsync();
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
                    Data = posts.Select(post => new
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        CategoryName = post.Category.Name,
                        UserId = post.UserId,
                        UserName = post.User.UserName,
                        Comments = post.Comments.Select(comment => new
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreatedAt = comment.CreatedAt,
                            UserId = comment.UserId,
                            Username = comment.User.UserName
                        })
                    })
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

        [HttpGet("NewAll")]
        public async Task<IActionResult> GetAll2()
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetAllAsync(
                    //predicate: p => p.CategoryId == 2
                   includes: new Expression<Func<Post,Object>>[]
                   {
                       p => p.Comments,
                       P => P.User,
                       p => p.Category
                   }
                    );
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
                    Data = posts.Select(post => new
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        CategoryName = post.Category.Name,
                        UserId = post.UserId,
                        UserName = post.User.UserName,
                        Comments = post.Comments.Select(comment => new
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreatedAt = comment.CreatedAt,
                            UserId = comment.UserId,
                            //Username = comment.User.UserName
                        })
                    })
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
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
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
                    Data = new
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        CategoryName = post.Category.Name,
                        UserId = post.UserId,
                        UserName = post.User.UserName,
                        Comments = post.Comments.Select(comment => new
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            CreatedAt = comment.CreatedAt,
                            UserId = comment.UserId,
                            Username = comment.User.UserName
                        })
                    }
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
                await _unitOfWork.Posts.CreateAsync(Post);
                await _unitOfWork.SaveAsync();
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
                var oldPost = await _unitOfWork.Posts.GetByIdAsync(int.Parse(postDTo.UserId));
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
                _unitOfWork.Posts.Update(oldPost);
                await _unitOfWork.SaveAsync();
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
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Post with ID {id} not found",
                    });
                }
                _unitOfWork.Posts.Delete(post);
                await _unitOfWork.SaveAsync();
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

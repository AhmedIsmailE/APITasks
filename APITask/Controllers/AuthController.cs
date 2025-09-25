using API.Core.DTos;
using API.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // DI to use user table
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        // Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (ModelState.IsValid) {
                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                    };
                    IdentityResult Result = await _userManager.CreateAsync(user,model.Password);
                    if (Result.Succeeded) {
                        return StatusCode(201, new
                        {
                            StatusCode = 201,
                            Message = "User created successfully"
                        });
                    } 
                    else
                    {
                        foreach (var Error in Result.Errors) {
                            ModelState.AddModelError("Error", Error.Description);
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    StatusCode = 500,
                    Message = "Error occured while handling the data"
                });
            }
        }
        //Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
            
                if (ModelState.IsValid)
                {
                    // check Email
                    var User = await _userManager.FindByEmailAsync(model.Email);
                    if (User is not null)
                    {
                        // check password
                        if (await _userManager.CheckPasswordAsync(User, model.Password))
                        {
                            // Payload The data
                            var Claims = new List<Claim>();
                            // Custom Data
                            //Claims.Add(new Claim("TokenNum", "1"));
                            //  Pre Defined
                            // User Data
                            Claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id)); 
                            Claims.Add(new Claim(ClaimTypes.Name, User.UserName));  
                            Claims.Add(new Claim(ClaimTypes.Role, "User"));

                            // Unique Id for token
                            Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                            // User Roles
                            // get User Roles
                            var UserRoles = await _userManager.GetRolesAsync(User);
                            foreach (var role in UserRoles)
                                Claims.Add(new Claim(ClaimTypes.Role, role.ToString()));


                            // Generate Token
                            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                            // signingCredentials
                            var SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);


                            var Token = new JwtSecurityToken(
                                    claims: Claims,
                                    audience: _config["JWT:Audience"],
                                    issuer: _config["JWT:Issuer"],
                                    expires: DateTime.Now.AddMinutes(5),
                                    signingCredentials: SigningCredentials

                                );

                            var _token = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(Token),
                                Expiration = Token.ValidTo
                            };



                            return Ok(_token);
                        }
                        else
                        {
                            return Unauthorized();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid Email");
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }

    }
}

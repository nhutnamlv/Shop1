using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Services;
using System.Security.Claims;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;

        public AuthController(ShopDbContext shopDbContext, TokenService tokenService, PasswordService passwordService)
        {
            this._shopDbContext = shopDbContext;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        //Viết 1 cái until để hash password dùng bcrypt 
        //tạo 1 cái role controller có crud tạo sửa đọc xoá ;
        //tạo 1 cái role name là User và Admin
        // tạo github 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RequestRegisterDTO request)
        {
            // check email tồn tại
            var emailExists = await _shopDbContext.Users
                .AnyAsync(x => x.Email == request.Email);

            if (emailExists)
            {
                return BadRequest(new
                {
                    message = "Email already exists"
                });
            }

            // lấy role User
            var role = await _shopDbContext.Roles
                .FirstOrDefaultAsync(x => x.Name == "User");

            // tạo user
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,

                // demo học tập
                // thực tế nên hash password
                Password = _passwordService.HashPassword(request.Password),

                PhoneNumber = request.PhoneNumber,
                Address = request.Address,

                RoleId = role?.Id,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.Email
            };

            _shopDbContext.Users.Add(user);

            await _shopDbContext.SaveChangesAsync();

            // tạo access token
            var accessToken = _tokenService.GenerateAccessToken(user);

            // tạo refresh token
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            return Ok(new
            {
                message = "Register success",
                user,
                accessToken,
                refreshToken
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO DTO)
        {
            var user = await _shopDbContext.Users.FirstOrDefaultAsync(x =>x.Email == DTO.Email);
            if(user == null)
            {
                return NotFound(new
                {

                    Message="NotFound User"
                });
            }
            var PassWordValidate = _passwordService.VerifyPassword(DTO.Password, user.Password);
            if(PassWordValidate == false)
            {
                throw new BadHttpRequestException("Wrong PassWord");
            }
            //tạo acc
            var accessToken = _tokenService.GenerateAccessToken(user);
            //tạo re
            var refreshToken = _tokenService.GenerateRefreshToken(user);
            return Ok(new
            {
                message = "Login success",
                user,
                accessToken,
                refreshToken
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _shopDbContext.Users.FirstOrDefaultAsync(x=>x.Id == int.Parse(UserId));
            return Ok(user);
        }
    } 
}


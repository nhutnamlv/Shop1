using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Models;
using ShopApi.Services;
using Shop1.DTO;
namespace Shop1.Controllers
{
    public class AuthController : Controller
    {
        private readonly ShopDbContext _shopDbContext;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;

        public AuthController(ShopDbContext shopDbContext, TokenService tokenService, PasswordService passwordService)
        {
            this._shopDbContext = shopDbContext;
            _tokenService = tokenService;
            _passwordService= passwordService;
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
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                message = "Register success",
                user,
                accessToken,
                refreshToken
            });
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;

        public RoleController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] RequestRoleDTO request)
        {
            //check role tồn tại
            var RoleExist = await _shopDbContext.Roles
                .AnyAsync(x => x.Name == request.Name);
            if (RoleExist)
            {
                return BadRequest(new
                {
                    message = "Role already exists"
                });
            }
            //tạo role mới
            var role = new Role
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Roles.Add(role);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Role created successfully",
                data = role
            });
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRole()
        {
            //lấy tất cả role
            var roles = await _shopDbContext.Roles.ToListAsync();
            return Ok(new
            {
                message = "Get all roles successfully",
                data = roles
            });
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            //lấy role theo id
            var role = await _shopDbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found"
                });
            }
            return Ok(new
            {
                message = "Get role by id successfully",
                data = role
            });
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RequestRoleDTO request)
        {
            //lấy role theo id
            var role = await _shopDbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return NotFound(new
                {
                    message = "Role not found"
                });
            }
            //cập nhật role
            role.Name = request.Name;
            role.UpdatedAt = DateTime.UtcNow;
            role.UpdatedBy = "system";
            _shopDbContext.Roles.Update(role);// chỗ này chat kêu kh cần 
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Role updated successfully",
                data = role
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            // lấy role theo id
            var role= await _shopDbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if(role == null)
            {
                return NotFound(new
                {
                    message = "Role not found"
                });
            }    
            //xoá role 
            _shopDbContext.Roles.Remove(role);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Role deleted successfully",
                data = role
            });
        }
    }
}

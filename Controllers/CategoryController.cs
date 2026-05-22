using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;

        public CategoryController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] RequestCategoryDTO request)
        {
            //check tồn tại 
            var categoryExists = await _shopDbContext.Categories
                .AnyAsync(x => x.Name == request.Name);
            if (categoryExists) {
                return BadRequest(
                    new
                    {
                        message = "Category already exits"
                    });
            }
            // tạo mới
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Categories.Add(category);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Category created sucsessfully",
                data = category,
            });
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var categories = await _shopDbContext.Categories.ToListAsync();
            return Ok(new
            {
                message = "Get all category sucsessfully",
                data = categories
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _shopDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }
            return Ok(new
            {
                message = "Category updated successfully",
                data = category
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestCategoryDTO request)
        {
            var category = await _shopDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            }
            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = "system";
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "GetCategoryById sucsessfully",
                data = category
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _shopDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found"
                });
            } 
                _shopDbContext.Categories.Remove(category);
                await _shopDbContext.SaveChangesAsync();
                return Ok(new
                {
                    message = "Category remove sucsessfully",
                    data = category
                });
            }
        }
    } 

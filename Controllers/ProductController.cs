using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;
using System.Xml.Linq;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;
        public ProductController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] RequestProductDTO request)
        {
            var ProductExists = await _shopDbContext.Products
            .AnyAsync(x => x.Name == request.Name);
            if (ProductExists)
            {
                return BadRequest(new
                {
                    message = "Product already exists"
                });
            }
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Products.Add(product);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Product created sucessfully",
                data = product
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _shopDbContext.Products.ToListAsync();
            return Ok(new
            {
                message = "Get all product sucessfully",
                data = products
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _shopDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }
            return Ok(new
            {
                message = "Get product sucessfully",
                data = product
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductById(int id, [FromBody] RequestProductDTO request)
        {
            var product = await _shopDbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found",
                });
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.ImageUrl = request.ImageUrl;
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = "system";
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Put product sucessfully",
                data = product
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _shopDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }
                _shopDbContext.Products.Remove(product);
                await _shopDbContext.SaveChangesAsync();
                return Ok(new
                {
                    message = "Product remove sucessfully",
                    data=product
                });
            }
        }
    }


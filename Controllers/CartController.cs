using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CartController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;
        public CartController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] RequestPaymenDTO request)
        {
            var CartExists = await _shopDbContext.Carts
                .AnyAsync(x => x.UserId == request.UserId);
            if (CartExists)
            {
                return BadRequest(new
                {
                    message = "CartExists already"
                });
            }
            var Cart = new Cart
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Carts.Add(Cart);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Created Cart sucsessfully ",
                data = Cart
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCart()
        {
            var carts = await _shopDbContext.Carts.ToListAsync();
            return Ok(new
            {
                message = "Get Cart sucsessfully",
                data = carts
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            var cart = await _shopDbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (cart == null)
            {
                return NotFound(new
                {
                    message = "Cart not found"
                });
            }    
            return Ok(new
            {
                message = "Get Cart sucsessfully",
                data = cart
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] RequestPaymenDTO request)
        {
            var cart = await _shopDbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (cart == null)
            {
                return NotFound(new
                {
                    message = "Cart not found"
                });
            }
            cart.UserId = request.UserId;
            cart.CreatedAt = DateTime.UtcNow;
            cart.CreatedBy = "system";
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Update Cart sucsessfully",
                data = cart
            });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveCart(int id)
        {
            var cart = await _shopDbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (cart == null)
            {
                return NotFound(new
                {
                    message = "Cart not found"
                });
            }
            _shopDbContext.Carts.Remove(cart);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "remove Cart sucsessfully",
                data = cart
            });
        }
    }
}

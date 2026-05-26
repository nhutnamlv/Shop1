using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;
using System.Net;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrderController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;
        public OrderController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] RequestOrderDTO request)
        {
            var OrderExists = await _shopDbContext.Orders
                .AnyAsync(x => x.UserId == request.UserId);
            if (OrderExists)
            {
                return BadRequest(new
                {
                    message = "OrderExists already  "
                });
            }
            var order = new Order
            {
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Orders.Add(order);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Create order successfully",
                data = order
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var orders = await _shopDbContext.Orders.ToListAsync();
            return Ok(new
            {
                message = "GetAll order successfully",
                data = orders
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _shopDbContext.Orders.FirstOrDefaultAsync(x=>x.Id==id);
            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found  "
                });
            }
            return Ok(new
            {
                message = "Get order successfully",
                data = order
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] RequestOrderDTO request)
        {
            var order = await _shopDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found  "
                });
            }
            order.UserId = request.UserId;
            order.TotalPrice = request.TotalPrice;
            order.Address = request.Address;
            order.PhoneNumber = request.PhoneNumber;
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = "system";
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Update order successfully",
                data = order
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            var order = await _shopDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found  "
                });
            }
            _shopDbContext.Orders.Remove(order);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Remove order successfully",
                data = order
            });
        }
    }
}

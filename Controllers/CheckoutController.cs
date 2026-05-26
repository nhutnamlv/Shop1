using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ShopDbContext _shopDbContext;

        public CheckoutController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] RequestCheckoutDTO request)
        {
            var userExists = await _shopDbContext.Users
                .AnyAsync(x => x.Id == request.UserId);

            if (userExists == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            var order = new Order
            {
                UserId = request.UserId,
                TotalPrice = 0,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Status = 0,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _shopDbContext.Orders.Add(order);

            await _shopDbContext.SaveChangesAsync();

            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = 0,
                PaidAt = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _shopDbContext.Payments.Add(payment);

            await _shopDbContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Checkout successfully",
                order = order,
                payment = payment
            });
        }
    }
}
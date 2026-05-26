using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop1.DTO;
using ShopApi.Data;
using ShopApi.Models;

namespace Shop1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class PaymentController : ControllerBase
    {
        private readonly ShopDbContext  _shopDbContext;
        public PaymentController(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] RequestPaymentDTO request)
        {
            var PaymentExists = await _shopDbContext.Payments
                .AnyAsync(x => x.OrderId == request.OrderId);
            if(PaymentExists)
            {
                return BadRequest(new
                {
                    message = "PaymentExists already exists"
                });
            }
            var payment = new Payment
            {
                OrderId = request.OrderId,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = request.PaymentStatus,
                PaidAt = request.PaidAt,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };
            _shopDbContext.Payments.Add(payment);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Payment created sucessfully",
                data = payment
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPayment()
        {
            var payments = await _shopDbContext.Payments.ToListAsync();
            return Ok(new
            {
                message = "Get all payments sucessfully",
                data = payments
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _shopDbContext.Payments.FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found"
                });
            }
            return Ok(new
            {
                message = "Get Payment sucessfully",
                data = payment 
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentById(int id, [FromBody] RequestPaymentDTO request)
        {
            var payment = await _shopDbContext.Payments
            .FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found",
                });
            }
            payment.OrderId = request.OrderId;
            payment.PaymentMethod = request.PaymentMethod;
            payment.PaymentStatus = request.PaymentStatus;
            payment.PaidAt = request.PaidAt;
            payment.UpdatedAt = DateTime.UtcNow;
            payment.UpdatedBy = "system";
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Update payment sucessfully",
                data = payment
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _shopDbContext.Payments.FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found"
                });
            }
            _shopDbContext.Payments.Remove(payment);
            await _shopDbContext.SaveChangesAsync();
            return Ok(new
            {
                message = "Product remove sucessfully",
                data = payment
            });
        }
    }
}


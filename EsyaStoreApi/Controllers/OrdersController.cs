using EsyaStore.Data.Context;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders() {
            var orders=await _context.orders.ToListAsync();
            if (orders is null) { return NotFound(); }
            var ordersDTO = new List<OrderDetailsDTO>();
            foreach (var order in orders) {
                var EachOrder = new OrderDetailsDTO()
                {
                    Id = order.Id,
                    OrderNo = order.OrderNo,
                    OrderDate = order.OrderDate,
                    OrderPrice = order.OrderPrice,
                    Address = order.Address,
                    OrderStatus = order.OrderStatus,
                    ProductNAme = await _context.products.Where(p => p.Id == order.ProductId).Select(p => p.ProductName).FirstOrDefaultAsync(),
                    Quantity = order.Quantity,
                    UserName=await _context.users.Where(u=>u.Id==order.UserId).Select(u=>u.Name).FirstOrDefaultAsync()
                };
                ordersDTO.Add(EachOrder);
            }

            return Ok(ordersDTO);
            
        }
    
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrderByUser(int userId) {
            var order =await _context.orders.Where(o => o.UserId == userId).FirstOrDefaultAsync();
            if (order is null) { return NotFound(); }
            return Ok(order);
        }
    }
}


 
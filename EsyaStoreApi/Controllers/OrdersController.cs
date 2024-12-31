using EsyaStore.Data.Context;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllOrders() {
            var orders=_context.orders.ToList();
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
                    ProductNAme = _context.products.Where(p => p.Id == order.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                    Quantity = order.Quantity,
                    UserName=_context.users.Where(u=>u.Id==order.UserId).Select(u=>u.Name).FirstOrDefault()
                };
                ordersDTO.Add(EachOrder);
            }

            return Ok(ordersDTO);
            
        }
    
        [HttpGet("{userId}")]
        public IActionResult GetOrderByUser(int userId) {
            var order = _context.orders.Where(o => o.UserId == userId);
            if (order is null) { return NotFound(); }
            return Ok(order);
        }
    }
}



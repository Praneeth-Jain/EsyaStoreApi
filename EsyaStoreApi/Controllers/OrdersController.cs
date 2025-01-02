using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "UsernSellerPolicy")]
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
    
        [HttpGet("user/{userId}")]
        [Authorize(Policy ="UserPolicy")]
        public async Task<IActionResult> GetOrderByUser(int userId) {
            var order =await _context.orders.Where(o => o.UserId == userId).FirstOrDefaultAsync();
            if (order is null) { return NotFound(); }
            return Ok(order);
        }
        
        [HttpGet("seller/{SellerId}")]
        [Authorize(Policy ="SellerPolicy")]
        public async Task<IActionResult> GetOrderBySeller(int SellerId) {
            var productList =await _context.products.Where(p => p.SellerId == SellerId).ToListAsync();
            var orderList = new List<Order>();
            foreach (var product in productList) {
                var order = await _context.orders.Where(o => o.ProductId == product.Id).ToListAsync();
                foreach (var orderItem in order)
                {
                    orderList.Add(orderItem);
                }
                    }
            if (orderList is null) { return NotFound(); }
            return Ok(orderList);
        }
    }
}


 
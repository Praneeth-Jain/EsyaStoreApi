using System.Security.Claims;
using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;  
        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }
  
        [HttpGet("{userID}")]
        [Authorize(Policy ="UserPolicy")]
        public async Task <IActionResult> GetCartItems(int userid) {
            var user=await _context.users.FindAsync(userid);
            if (user == null) { return NotFound("User not found."); };
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user.Id.ToString() != userClaim)
            {
                return Unauthorized("You are not authorized to access this cart.");
            }
            var cartitems=await _context.cart.Where(c=>c.UserId == userid).ToListAsync();
            return Ok(cartitems);
        }

        [HttpPost]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> AddItemstoCart(AddtoCartDTO cartDto)
        {
            var user =await _context.users.FindAsync(cartDto.UserId);
            if (user == null) { return NotFound("The given user is not found."); };
            var product=await _context.products.FindAsync(cartDto.ProductId);
            if (product == null) { return NotFound("The product specified is not found."); };
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user.Id.ToString() != userClaim)
            {
                return Unauthorized("You are not authorized to add item to this cart.");
            }
            var NewCart = new Cart
            {
                ProductId = product.Id,
                UserId = user.Id
            };
            await _context.cart.AddAsync(NewCart);
            await _context.SaveChangesAsync();
            return Created();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="UserPolicy")]
        public async Task<IActionResult> DeleteCartItem(int id) { 
            var userClaim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItem = await _context.cart.FindAsync(id);
            if (cartItem == null) { return NotFound("Cart not found."); };
            if (cartItem.UserId.ToString() != userClaim) { return Unauthorized("You are not authorized to remove item from this cart."); };
            _context.cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

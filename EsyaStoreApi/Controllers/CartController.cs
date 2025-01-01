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
    public class CartController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{userID}")]
        [Authorize(Policy ="UserPolicy")]
        public IActionResult GetCartItems(int userid) {
            var user=_context.users.Find(userid);
            if (user == null) { return NotFound("User not found."); };
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user.Id.ToString() != userClaim)
            {
                return Unauthorized("You are not authorized to access this cart.");
            }
            var cartitems=_context.cart.Where(c=>c.UserId == userid).ToList();
            return Ok(cartitems);
        }

        [HttpPost]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult AddItemstoCart(AddtoCartDTO cartDto)
        {
            var user = _context.users.Find(cartDto.UserId);
            if (user == null) { return NotFound("The given user is not found."); };
            var product=_context.products.Find(cartDto.ProductId);
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
            _context.cart.Add(NewCart);
            _context.SaveChanges();
            return Created();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy ="UserPolicy")]
        public IActionResult DeleteCartItem(int id) { 
            var userClaim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItem = _context.cart.Find(id);
            if (cartItem == null) { return NotFound("Cart not found."); };
            if (cartItem.UserId.ToString() != userClaim) { return Unauthorized("You are not authorized to remove item from this cart."); };
            _context.cart.Remove(cartItem);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

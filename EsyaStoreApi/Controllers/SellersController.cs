using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SellersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllSellers() { 
            var seller=await _context.sellers.ToListAsync();
            if(seller is null)
            {
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeller(int id)
        {
            var seller =await _context.sellers.FindAsync(id);
            if (seller is null) { 
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpPost]
        public async Task<IActionResult> PostSeller(CreateSellerDTO newSeller)
        {
            var seller = new Sellers
            {
                Email = newSeller.Email,
                Name = newSeller.Name,
                Password = newSeller.Password,
                Contact = newSeller.Contact,
                Location = newSeller.Location,
                isActiveSeller = newSeller.isActiveSeller
            };
            _context.sellers.Add(seller);
            await _context.SaveChangesAsync());
            return CreatedAtAction(nameof(GetSeller),new {id=seller.Id}, seller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeller(int id,UpdateSellerDTO editSeller)
        {
            var seller=await _context.sellers.FindAsync(id);
            if (seller is null)
            {
                return NotFound();
            }
            seller.Name = editSeller.Name;
            seller.Password = editSeller.Password;
            seller.Location= editSeller.Location;
            seller.isActiveSeller = editSeller.isActiveSeller;
            seller.Contact = editSeller.Contact;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(int id) {
            var seller =await _context.sellers.FindAsync(id);
            if(seller is null)
            {
                return NotFound();
            }
            _context.sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

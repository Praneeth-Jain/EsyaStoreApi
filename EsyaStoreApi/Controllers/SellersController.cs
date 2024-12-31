using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllSellers() { 
            var seller=_context.sellers.ToList();
            if(seller is null)
            {
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpGet("{id}")]
        public IActionResult GetSeller(int id)
        {
            var seller = _context.sellers.Find(id);
            if (seller is null) { 
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpPost]
        public IActionResult PostSeller(CreateSellerDTO newSeller)
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
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSeller),new {id=seller.Id}, seller);
        }

        [HttpPut("{id}")]
        public IActionResult PutSeller(int id,UpdateSellerDTO editSeller)
        {
            var seller= _context.sellers.Find(id);
            if (seller is null)
            {
                return NotFound();
            }
            seller.Name = editSeller.Name;
            seller.Password = editSeller.Password;
            seller.Location= editSeller.Location;
            seller.isActiveSeller = editSeller.isActiveSeller;
            seller.Contact = editSeller.Contact;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeller(int id) {
            var seller = _context.sellers.Find(id);
            if(seller is null)
            {
                return NotFound();
            }
            _context.sellers.Remove(seller);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

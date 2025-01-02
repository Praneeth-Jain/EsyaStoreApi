using System.Security.Claims;
using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Extensions;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly TokenGenerator _jwtTokenGenerator;

        public SellersController(ApplicationDbContext context,TokenGenerator tokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginSeller(LoginDto loginDto)
        {
            var seller = await _context.sellers.FirstOrDefaultAsync(s => s.Email == loginDto.Email && s.Password == loginDto.Password);
            if (seller is null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var token = _jwtTokenGenerator.CreateToken(seller.Name, seller.Email, seller.Id, loginDto.UserType);

            return Ok(new { token });


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
        public async Task<IActionResult> Register(CreateSellerDTO newSeller)
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
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeller),new {id=seller.Id}, seller);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> EditSeller(int id,UpdateSellerDTO editSeller)
        {
            var sellerClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (sellerClaim != id.ToString()) { return Unauthorized("You are not authorized to edit."); };
            var seller =await _context.sellers.FindAsync(id);
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
        [Authorize(Policy ="SellerPolicy")]
        public async Task<IActionResult> DeleteSeller(int id) {
            var sellerClaim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (sellerClaim != id.ToString()) { return Unauthorized("You are not authorized to delete."); };
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

﻿using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products=await _context.products.ToListAsync();
            if(products is null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id) {
            var product =await _context.products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(CreateProductDTO newProduct)
        {
            var product = new Products
            {
                ProductName = newProduct.ProductName,
                ProductDescription = newProduct.ProductDescription,
                ProductCategory = newProduct.ProductCategory,
                ProdImgUrl = newProduct.ProdImgUrl,
                Manufacturer = newProduct.Manufacturer,
                ProductPrice = newProduct.ProductPrice,
                Discount = newProduct.Discount,
                FinalPrice = newProduct.ProductPrice - ((newProduct.ProductPrice * newProduct.Discount)/100),
                ProductQuantity = newProduct.ProductQuantity,
                SellerId = newProduct.SellerId
            };
            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct),new {id=product.Id},product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id,UpdateProductDTO editProduct) { 
            var product=await _context.products.FindAsync(id);
            if(product is null) { return NotFound(); }
            product.ProductName=editProduct.ProductName;
            product.ProductDescription=editProduct.ProductDescription;
            product.ProductCategory=editProduct.ProductCategory;
            product.ProductPrice=editProduct.ProductPrice;
            product.Discount=editProduct.Discount;
            product.Manufacturer=editProduct.Manufacturer;
            product.ProductQuantity=editProduct.ProductQuantity;
            product.FinalPrice = editProduct.ProductPrice - ((editProduct.ProductPrice * editProduct.Discount)/100);
            product.ProdImgUrl=editProduct.ProdImgUrl;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) {
            var product=await _context.products.FindAsync(id);
            if (product is null) { return NotFound(); }
            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

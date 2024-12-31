﻿using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class CreateProductDTO
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int ProductQuantity { get; set; }

        [Required]
        public string ProductCategory { get; set; }

        public string Manufacturer { get; set; }

        public string ProdImgUrl { get; set; }


        public int SellerId { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; } = 0;

      
    }
}

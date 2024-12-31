using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class OrderDetailsDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderNo { get; set; }

        [Required]
        public string UserName{ get; set; }

        [Required]
        public string ProductNAme { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int OrderStatus { get; set; } = 1;

        public int Quantity { get; set; } = 1;

        [Required, MaxLength(150)]
        public string Address { get; set; }

        public decimal OrderPrice { get; set; }
    }
}

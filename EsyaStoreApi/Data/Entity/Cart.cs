using System.ComponentModel.DataAnnotations;

namespace EsyaStore.Data.Entity
{
    public class Cart

    
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}

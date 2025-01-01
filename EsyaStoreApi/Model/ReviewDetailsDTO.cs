using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class ReviewDetailsDTO
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, Range(1, 5)]
        public int Stars { get; set; }

        public string ReviewDescription { get; set; }

        public DateTime ReviewDate { get; set; } 
    }
}

using System.ComponentModel.DataAnnotations;

namespace EsyaStore.Data.Entity
{
    public class Reviews
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required,Range(1,5)]
        public int Stars {  get; set; }
        
        public string ReviewDescription { get; set; }

        public DateTime ReviewDate { get; set; }= DateTime.Now;
    }
}

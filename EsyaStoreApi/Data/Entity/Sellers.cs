using System.ComponentModel.DataAnnotations;

namespace EsyaStore.Data.Entity
{
    public class Sellers
    {

        [Key]
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Name { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string Contact { get; set; }

        public string Location { get; set; }

        public int isActiveSeller { get; set; } = 1;
    }
}

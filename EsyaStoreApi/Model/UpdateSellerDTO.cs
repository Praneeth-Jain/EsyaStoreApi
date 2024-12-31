using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class UpdateSellerDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string Contact { get; set; }

        public string Location { get; set; }

        public int isActiveSeller { get; set; } = 1;
    }
}

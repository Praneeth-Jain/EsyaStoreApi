using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class UpdateUserDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string Contact { get; set; }

        public int isActiveUser { get; set; } = 1;
    }
}

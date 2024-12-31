using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class CreateUserDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string Contact { get; set; }

        public int isActiveUser { get; set; } = 1;
    }
}


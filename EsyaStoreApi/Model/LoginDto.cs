using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        public string UserType { get; set; }
    }
}

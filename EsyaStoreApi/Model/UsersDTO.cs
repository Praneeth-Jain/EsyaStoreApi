using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class UsersDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Contact { get; set; }

        public string isActiveUser { get; set; }
    }
}

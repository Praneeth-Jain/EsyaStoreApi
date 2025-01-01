using System.ComponentModel.DataAnnotations;

namespace EsyaStoreApi.Model
{
    public class AddtoCartDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

    }
}

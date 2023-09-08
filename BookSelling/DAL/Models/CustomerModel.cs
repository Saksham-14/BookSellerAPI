using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class CustomerModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}

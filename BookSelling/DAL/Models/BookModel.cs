using DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class BookModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public OfferStatus OfferStatus { get; set; }
    }
}

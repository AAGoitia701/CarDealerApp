using System.ComponentModel.DataAnnotations;

namespace CarDealerApp.Data
{
    public class Owner
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string CardId { get; set; }
        [Required]
        public string FullName {  get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int PostalCode { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

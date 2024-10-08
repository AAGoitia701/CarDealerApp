using System.ComponentModel.DataAnnotations;

namespace CarDealerApp.Data
{
    public class Owner
    {
        [Key]
        public int Id { get; set; } 
        [Required(ErrorMessage = "Card ID is a required field")]
        public string CardId { get; set; }
        [Required(ErrorMessage = "Full Name is a required field")]
        public string FullName {  get; set; }
        [Required(ErrorMessage = "Date of birth is a required field")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateBirth { get; set; }
        [Required(ErrorMessage = "Phone number is a required field")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Postal code is a required field")]
        public int PostalCode { get; set; }
        [Required(ErrorMessage = "Email is a required field")]
        public string Email { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealerApp.Data
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Identification Number")]
        [Required(ErrorMessage = "Card ID is a required field")]
        public string CardId { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Full Name is a required field")]
        public string FullName {  get; set; }

        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Date of birth is a required field")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateBirth { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone number is a required field")]
        public string PhoneNumber { get; set; }

        [DisplayName("Postal Code")]
        [Required(ErrorMessage = "Postal code is a required field")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "Email is a required field")]
        public string Email { get; set; }

        public List<Car>? ListCars { get; set; } //can have many or no cars
    }
}

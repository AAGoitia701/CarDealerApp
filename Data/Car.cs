using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarDealerApp.Data
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        [Required]
        public string LicencePlate { get; set; }

        [Required]
        public string Color { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "yyyy / MMMM")]
        public DateTime DateTime { get; set; }
        public string? Owner { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealerApp.Data
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        [DisplayName("Licence Plate")]
        [Required(ErrorMessage = "Licence Plate is a required field")]
        public string LicencePlate { get; set; }

        [Required(ErrorMessage = "Color is a required field")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Model is a required field")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Brand is a required field")]
        public string Brand { get; set; }

        [DisplayName("Year")]
        [Required(ErrorMessage = "Year is a required field")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateTime { get; set; }

        [DisplayName("Image")]
        public string? ImgUrl { get; set; }

        public int? OwnerId { get; set; } //can have one or no owner
        public Owner? Owner { get; set; }

    }
}

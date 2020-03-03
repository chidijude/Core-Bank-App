using Express.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
    public enum Gender    {        Male = 1, Female = 2    }

   
    public class Customer
    {

        [Display(Name = "Customer ID")]
        public int ID { get; set; }

        
        [StringLength(255)]
        [Required(ErrorMessage = "Name field is required")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should contain characters and white spaces only"), MaxLength(40)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }


        public Branch Branch { get; set; }

        
        [Required]
        public Gender Gender { get; set; }

        [DataType(DataType.Text), MaxLength(100)]
        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number")]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid phone number"), MinLength(11), MaxLength(16)]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address"), MaxLength(100)]
        public string Email { get; set; }
    }
}
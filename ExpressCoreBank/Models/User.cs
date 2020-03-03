using Express.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
   
    public class User:ApplicationUser
    {
        public override string Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Last Name field is required")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should contain characters and white spaces only"), MaxLength(40)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number")]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid phone number"), MinLength(11), MaxLength(16)]
        [Display(Name = "Phone number")]
        public override string PhoneNumber { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name field is required")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "user Name should contain characters and white spaces only"), MaxLength(40)]
        [Display(Name = "User Name")]
        public override string UserName { get; set; }



        [StringLength(255)]
        [Required(ErrorMessage = "Last Name field is required")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Full name should contain characters and white spaces only"), MaxLength(40)]
        [Display(Name ="Last name")]
        public string LastName { get; set; }


        [Display(Name= "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [Display(Name ="Select Branch")]
        public int? BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        [Display(Name ="Roles")]
        public string RoleID { get; set; }
        public virtual Role Role { get; internal set; }


    }
}
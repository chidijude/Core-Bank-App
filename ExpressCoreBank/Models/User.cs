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

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last name")]
        public string LastName { get; set; }

        [Display(Name= "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        

        public Branch Branch { get; set; }


    }
}
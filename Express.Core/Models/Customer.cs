using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public enum Branch
    {
        BranchA,
        BrachB,
        BranchC
    }
    public class Customer
    {


        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }


        public Branch Branch { get; set; }

        
        [Required]
        public Gender Gender { get; set; }


        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
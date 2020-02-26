using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
    public class CustomerAccount
    {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }


        [Required]
        public AccountType AccountType { get; set; }

        public int CustomerId { get; set; }

        public int AccountTypeId { get; set; }

        [Required]
        public string AccountName { get; set; }

        public long? AccountBalance { get; set; }

        public Branch Branch { get; set; }
    }
}
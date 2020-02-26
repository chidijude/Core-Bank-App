using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
    public enum AccountNameId
    {
        Savings = 1100,

        Current = 2200,

        Loan = 3300
    }
    public class AccountType
    {

        public byte Id { get; set; }

        public byte? Interest { get; set; }

        public String Name { get; set; }

        public int AccountTypeId { get; set; }

    }
}
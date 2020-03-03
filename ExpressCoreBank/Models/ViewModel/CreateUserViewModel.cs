using Express.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
    public class CreateUserViewModel
    {

        public IEnumerable<Branch> Branches { get; set; }
        public User User { get; set; }
    }
}
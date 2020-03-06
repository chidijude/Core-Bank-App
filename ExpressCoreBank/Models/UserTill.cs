 using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public class UserTill
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Select a User")]
        [Display(Name = "User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Select a Till Account")]
        [Display(Name = "Till")]

        public int GlAccountID { get; set; }
        public virtual GlAccount GlAccount { get; set; }
    }
}

using Express.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpressLogic
{
    public class TillToUser
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Select a User")]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Select a Till Account")]
        [Display(Name = "Till")]
        public int GlAccountID { get; set; }
        public virtual GlAccount GlAccount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.ViewModels.CustomerAccountViewModel
{
    public class CreateAccountViewModel
    {
       

            [Required(ErrorMessage = "Account name is required")]
            [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Account name should only contain characters and white spaces"), MaxLength(40)]
            [Display(Name = "Account Name")]
            public string AccountName { get; set; }

            [Required(ErrorMessage = "Please select a branch")]
            [Display(Name = "Branch")]
            public int BranchID { get; set; }

            [Required]
            public int CustomerID { get; set; }
        
    }
}

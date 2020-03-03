using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public class GlAccount
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Account name is required"), MaxLength(40)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Code")]
        public long CodeNumber { get; set; }     ///(1 for Assets, 2 for Liablities, 3 for Capital, 4 for Income and 5 for Expenses)! Are these categories??

        [Display(Name = "Account Balance")]
        public decimal AccountBalance { get; set; } 

        [Required(ErrorMessage = "Please select a GL category")]
        [Display(Name = "Category")]
        public int GlCategoryID { get; set; }

        public virtual GlCategory GlCategory { get; set; }

        [Required(ErrorMessage = "Please select a branch")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }
        public virtual Branch Branch { get; set; }
    }
}

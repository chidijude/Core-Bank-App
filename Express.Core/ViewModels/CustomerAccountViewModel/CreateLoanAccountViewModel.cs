using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.ViewModels.CustomerAccountViewModel
{
    public class CreateLoanAccountViewModel
    {
       
            [Required(ErrorMessage = "Account name is required")]
            [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Account name should only contain characters and white spaces"), MaxLength(40)]
            [Display(Name = "Account Name")]
            public string AccountName { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Range(1000.00, (double)decimal.MaxValue, ErrorMessage = "Loan amount must be between #1,000 and a maximum reasonable amount")]
            [Display(Name = "Loan Amount")]
            public decimal LoanAmount { get; set; }

            [Required]
            [Display(Name = "Terms of loan")]
            public TermsOfLoan TermsOfLoan { get; set; }

            [Required(ErrorMessage = "Number of years is required")]
            [Range(0.084, 1000.0)]
            [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
            [Display(Name = "Number of years")]
            public double NumberOfYears { get; set; }

            [Required(ErrorMessage = "Interest rate is required")]
            [Display(Name = "Interest Rate")]
            [Range(0, 100)]
            [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
            public double InterestRate { get; set; }

            [Required(ErrorMessage = "Please select a branch")]
            [Display(Name = "Branch")]
            public int BranchID { get; set; }

            [Required]
            public int CustomerID { get; set; }

            [Required]
            [Display(Name = "Servicing Account Number")]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Account number should only contain numbers"), MinLength(10), MaxLength(10)]
            public string ServicingAccountNumber { get; set; }
        
    }
}

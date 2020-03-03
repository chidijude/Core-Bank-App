using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public class AccountConfiguration
    {
        public int ID { get; set; }
        [Display(Name = "Business Status")]
        public bool IsBusinessOpen { get; set; }

        public DateTime FinancialDate { get; set; }

        [Display(Name = "Savings Credit Interest Rate")]
        [Range(0, 100)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid input")]
        public double SavingsCreditInterestRate { get; set; }

        [Display(Name = "Savings Minimum Balance")]
        [Range(0, (double)decimal.MaxValue)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid input for minimum balance")]
        public decimal SavingsMinimumBalance { get; set; }

        [Display(Name = "Select Interest Expense GL")]
        public int? SavingsInterestExpenseGlID { get; set; }
        public virtual GlAccount SavingsInterestExpenseGl { get; set; }

        [Display(Name = "Select Interest Payable GL")]
        public int? SavingsInterestPayableGlID { get; set; }
        public virtual GlAccount SavingsInterestPayableGl { get; set; }    


        //current account side
        [Display(Name = "Current Credit Interest Rate")]
        [Range(0, 100)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public double CurrentCreditInterestRate { get; set; }

        [Display(Name = "Current Minimum Balance")]
        [Range(0, (double)decimal.MaxValue)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal CurrentMinimumBalance { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "COT")]
        [Range(0.00, 1000.00)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal CurrentCot { get; set; }        //Commission On Turnover

        [Display(Name = "Select Interest Expense GL")]
        public int? CurrentInterestExpenseGlID { get; set; }
        public virtual GlAccount CurrentInterestExpenseGl { get; set; }

        [Display(Name = "Select COT Income GL")]
        public int? CurrentCotIncomeGlID { get; set; }
        public virtual GlAccount CurrentCotIncomeGl { get; set; }

        [Display(Name = "Select Interest Payable GL")]
        public int? CurrentInterestPayableGlID { get; set; }
        public virtual GlAccount CurrentInterestPayableGl { get; set; }


        //Loan account side
        [Display(Name = "Loan Debit Interest Rate")]
        [Range(0, 100)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public double LoanDebitInterestRate { get; set; }

        [Display(Name = "Select Interest Income GL")]
        public int? LoanInterestIncomeGlID { get; set; }
        public virtual GlAccount LoanInterestIncomeGl { get; set; }

        [Display(Name = "Select Interest Expense GL")]
        public int? LoanInterestExpenseGLID { get; set; }
        public virtual GlAccount LoanInterestExpenseGl { get; set; }        //Expense: from where the loan is disbursed

        [Display(Name = "Select Interest Receivable GL")]
        public int? LoanInterestReceivableGlID { get; set; }
        public virtual GlAccount LoanInterestReceivableGl { get; set; }     //Asset
    }
}

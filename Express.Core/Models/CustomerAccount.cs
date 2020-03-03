using Express.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Models
{
   
        public enum AccountType
        {
            Savings = 1100, Current = 2200, Loan = 3300
        }
        public enum AccountStatus
        {
            Active, Closed
        }
        public enum TermsOfLoan
        {
            Fixed, Reducing
        }
        public class CustomerAccount
        {
            public int Id { get; set; }
            [Display(Name = "Account Number")]
            public long AccountNumber { get; set; }

            [Required(ErrorMessage = "Account name is required")]
            [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Account name should contain characters and white spaces only"), MaxLength(40)]
            [Display(Name = "Account Name")]
            public string AccountName { get; set; }

            [Required(ErrorMessage = "Please select a branch")]
            [Display(Name = "Branch")]
            public int BranchID { get; set; }
            public virtual Branch Branch { get; set; }


            [Display(Name = "Account Balance")]
            [DataType(DataType.Currency)]
            public decimal AccountBalance { get; set; }

            [Display(Name = "Date Created")]
            public DateTime DateCreated { get; set; }

            [Display(Name = "Loan duration")]
            public int DaysCount { get; set; }      //counts the number of days (at EOD) from account creation, (esp for loan accounts)

            [Display(Name = "Savings/Current/Loan Interest Accrued")]
            public decimal dailyInterestAccrued { get; set; }

            [Display(Name = "Current COT Accrued")]
            public decimal dailyCOTAccrued { get; set; }

            [Range(0, 100, ErrorMessage = "0% - 100%")]
            [Display(Name = "Loan Interest Rate Per Month")]
            public decimal LoanInterestRatePerMonth { get; set; }

            [Display(Name = "Account Type")]
            [Required(ErrorMessage = "You must select an account type")]
            public AccountType AccountType { get; set; }

            [Display(Name = "Account Status")]
            public AccountStatus AccountStatus { get; set; }

            [Display(Name = "Savings/Current Withdrawal Count")]
            public int SavingsWithdrawalCount { get; set; }

            [Display(Name = "Current Lien")]
            public decimal CurrentLien { get; set; }       //holding amount

            [Display(Name = "Customer Id")]
            //[Required(ErrorMessage = "Select a customer"), MinLength(8), MaxLength(8)]
            //[RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid Customer Id")]
            public int CustomerID { get; set; }
            public virtual Customer Customer { get; set; }



            //for a loan account
            [DataType(DataType.Currency)]
            [Display(Name = "Loan Amount")]
            public decimal LoanAmount { get; set; }

            [Display(Name = "Monthly Repayment")]
            public decimal LoanMonthlyRepay { get; set; }

            [Display(Name = "Loan Monthly Interest Repay")]
            public decimal LoanMonthlyInterestRepay { get; set; }

            [Display(Name = "Loan Monthly Principal Repay")]
            public decimal LoanMonthlyPrincipalRepay { get; set; }

            [Display(Name = "Loan Principal Remaining")]
            public decimal LoanPrincipalRemaining { get; set; }

            [Display(Name = "Terms Of Loan")]
            public TermsOfLoan? TermsOfLoan { get; set; }

            //[RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid Account Number")]
            //[Display(Name = "Linked Account Number")]
            public int? ServicingAccountID { get; set; }
            public virtual CustomerAccount ServicingAccount { get; set; }
        }
}

using ExpressCoreBank.Models;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public class CustomerAccountLogic
    {
        ConfigurationRepository configRepo = new ConfigurationRepository();
        CustomerAccountRepository custActRepo = new CustomerAccountRepository();

        public long GenerateCustomerAccountNumber(AccountType actType, int accountHolderId)
        {
            //The account number has to relate with the customer's Id and the account type
            int startDigit = 0;
            switch (actType)
            {
                case AccountType.Savings:
                    startDigit = 11;
                    break;
                case AccountType.Current:
                    startDigit = 22;
                    break;
                case AccountType.Loan:
                    startDigit = 33;
                    break;
                default:
                    break;
            }//snd switch

            int id = accountHolderId;
            // getting the count of savings/current/loan accts already created for the customer
            int count = custActRepo.GetCountByCustomerActType(actType, id);
            long actNo = Convert.ToInt64(startDigit + (count + 1).ToString("D3") + id.ToString("D5")); 

            return actNo;
        }//end method

        public void ComputeFixedRepayment(CustomerAccount act, double nyears, double interestRate)
        {
            decimal totalAmountToRepay = 0;
            double nMonth = nyears * 12;
            double totalInterest = (interestRate)/100 * nMonth * (double)act.LoanAmount;
            totalAmountToRepay = (decimal)totalInterest + (decimal)act.LoanAmount;
            act.LoanMonthlyRepay = (totalAmountToRepay / (12 * (decimal)nyears));
            act.LoanMonthlyPrincipalRepay = Convert.ToDecimal((double)act.LoanAmount / nMonth);
            act.LoanMonthlyInterestRepay = Convert.ToDecimal(totalInterest / nMonth);
            act.LoanPrincipalRemaining = (decimal)act.LoanAmount;
        }

        public void ComputeReducingRepayment(CustomerAccount act, double nyears, double interestRate)
        {
            double rate = (interestRate)/100;
            double x = 1 - Math.Pow((1 + rate), -(nyears * 12));
            act.LoanMonthlyRepay = ((decimal)act.LoanAmount * (decimal)rate) / (decimal)x;

            act.LoanPrincipalRemaining = (decimal)act.LoanAmount;
            act.LoanMonthlyInterestRepay = (decimal)rate * act.LoanPrincipalRemaining;
            act.LoanMonthlyPrincipalRepay = act.LoanMonthlyRepay - act.LoanMonthlyInterestRepay;
        }

        public bool CustomerAccountHasSufficientBalance(CustomerAccount account, decimal amountToDebit)
        {
            var config = configRepo.GetFirst();
            if (account.AccountBalance >= amountToDebit + config.SavingsMinimumBalance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

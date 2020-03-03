using Express.Core.Models;
using Express.Core.ViewModels.FinancialReportViewModel;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class BalanceSheetRepository
    {
        GlAccountRepository glactRepo = new GlAccountRepository();
        CustomerAccountRepository custRepo = new CustomerAccountRepository();

        public List<GlAccount> GetAssetAccounts()
        {
            var allAssets = glactRepo.GetByMainCategory(MainGlCategory.Asset);

            GlAccount loanAsset = new GlAccount();
            loanAsset.AccountName = "Loan Accounts";
            var loanAccounts = custRepo.GetByType(AccountType.Loan);
            foreach (var act in loanAccounts)
            {
                loanAsset.AccountBalance += act.AccountBalance;
            }
            allAssets.Add(loanAsset);
            return allAssets;
        }

        public List<GlAccount> GetIncomeAccounts()
        {
            var allIncome = glactRepo.GetByMainCategory(MainGlCategory.Income);
            GlAccount totalIncome = new GlAccount();
            totalIncome.AccountName = "Total Income";
            decimal incomeSume = glactRepo.GetByMainCategory(MainGlCategory.Income).Sum(a => a.AccountBalance);
            totalIncome.AccountBalance = incomeSume;
            allIncome.Add(totalIncome);
            return allIncome;
        }
        public List<GlAccount> GetExpenseAccounts()
        {
            var allExpense = glactRepo.GetByMainCategory(MainGlCategory.Expenses);
            GlAccount totalExpense = new GlAccount();
            totalExpense.AccountName = "Total Expense";
            decimal ExpenseSume = glactRepo.GetByMainCategory(MainGlCategory.Expenses).Sum(a => a.AccountBalance);
            totalExpense.AccountBalance = ExpenseSume;
            allExpense.Add(totalExpense);
            return allExpense;
        }
        public List<GlAccount> GetCapitalAccounts()
        {
            var allCapitals = glactRepo.GetByMainCategory(MainGlCategory.Capital);
            //adding the "Amount Left" capitals--> Profit or loss expressed as (Income - Expense)
            GlAccount remainingCapital = new GlAccount();
            remainingCapital.AccountName = "Amount Left";
            decimal incomeSum = glactRepo.GetByMainCategory(MainGlCategory.Income).Sum(a => a.AccountBalance);
            decimal expenseSum = glactRepo.GetByMainCategory(MainGlCategory.Expenses).Sum(a => a.AccountBalance);
            remainingCapital.AccountBalance = incomeSum - expenseSum;
            allCapitals.Add(remainingCapital);

            return allCapitals;
        }
        public List<LiabilityViewModel> GetLiabilityAccounts()
        {
            var liability = glactRepo.GetByMainCategory(MainGlCategory.Liability);

            var allLiabilityAccounts = new List<LiabilityViewModel>();

            foreach (var account in liability)
            {
                var model = new LiabilityViewModel();
                model.AccountName = account.AccountName;
                model.Amount = account.AccountBalance;

                allLiabilityAccounts.Add(model);

            }
            //adding customer's savings and current accounts since they are liabilities to the bank           
            var savingsAccounts = custRepo.GetByType(AccountType.Savings);
            var savingsLiability = new LiabilityViewModel();
            savingsLiability.AccountName = "Savings Accounts";
            savingsLiability.Amount = savingsAccounts != null ? savingsAccounts.Sum(a => a.AccountBalance) : 0;

            var currentAccounts = custRepo.GetByType(AccountType.Current);
            var currentLiability = new LiabilityViewModel();
            currentLiability.AccountName = "Current Accounts";
            currentLiability.Amount = currentAccounts != null ? currentAccounts.Sum(a => a.AccountBalance) : 0;

            allLiabilityAccounts.Add(savingsLiability);
            allLiabilityAccounts.Add(currentLiability);
            return allLiabilityAccounts;
        }
    }
}
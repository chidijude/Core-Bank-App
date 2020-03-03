using Express.Core.Models;
using ExpressCoreBank.Models;
using ExpressCoreBank.Repositories;
using ExpressLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUCBA.Logic
{
    public class EodLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        ConfigurationRepository configRepo = new ConfigurationRepository();
        CustomerAccountRepository custActRepo = new CustomerAccountRepository();
        GlAccountRepository glRepo = new GlAccountRepository();

        BusinessLogic busLogic = new BusinessLogic();
        FinancialReportLogic frLogic = new FinancialReportLogic();

        AccountConfiguration config;
        DateTime today;

        public EodLogic()
        {
            config = db.AccountConfigurations.First();
            today = config.FinancialDate;
        }
        int[] daysInMonth = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        public string RunEOD()
        {
            string output = "";
            //check if all configurations are set
            try
            {
                if (busLogic.IsConfigurationSet())
                {
                    CloseBusiness();

                    ComputeSavingsInterestAccrued(); // to calculate interest on saving account.
                    ComputeCurrentInterestAccrued(); // to calculate interest on current account and the commision on turnover.
                    ComputeLoanInterestAccrued(); // to calculate interest the loan account customer is to pay.
                    SaveDailyIncomeAndExpenseBalance();     //to calculate Profit and Loss using the expense income entries.
                    
                    //var config = db.AccountConfiguration.First();
                    config.FinancialDate = config.FinancialDate.AddDays(1);        //increments the financial date at the EOD

                    db.Entry(config).State = EntityState.Modified;
                    db.SaveChanges();
                                         //Ensures all or none of the 5 steps above executes and gets saved                     
                    output = "End of Day Executed Successfully!";
                }
                else
                {
                    output = "Account Configurations are not set!";
                }
            }
            catch (Exception)
            {
                output = "An error occured while executing EOD";
            }
            return output;
        }
        public void CloseBusiness()
        {
            config.IsBusinessOpen = false;
            db.Entry(config).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void OpenBusiness()
        {
            config.IsBusinessOpen = true;
            db.Entry(config).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool isBusinessClosed()
        {
           
            if (config.IsBusinessOpen)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// to sum up interrest and pay to approprait account
        /// </summary>
        public void ComputeSavingsInterestAccrued()
        {
            int presentDay = today.Day;     //1 to totalDays in the month as specified in days in month
            int presentMonth = today.Month;     //1 to 12, stating the total number of months in a year
            int daysRemaining = 0;  //to calculate days left till end of month
            if (custActRepo.AnyAccountOfType(AccountType.Savings))
            {
                //var allSavings = custActRepo.GetByType(AccountType.Savings).ToList();
               var allSavings = db.CustomerAccounts.Where(a => a.AccountType == AccountType.Savings).ToList();
                //foreach savings account
            foreach (var account in allSavings)
            {
                //get the no of days remaining in this month
                daysRemaining = daysInMonth[presentMonth - 1] - presentDay + 1;     //+1 because we havent computed for today
                decimal interestRemainingForTheMonth = account.AccountBalance * (decimal)config.SavingsCreditInterestRate * daysRemaining / daysInMonth[presentMonth - 1]; //using I = PRT, where R is the rate per month.
                //calculate today's interest and add it to the account's dailyInterestAccrued
                decimal todaysInterest = interestRemainingForTheMonth / daysRemaining;
                account.dailyInterestAccrued += todaysInterest;     //increments till month end. Disbursed if withdrawal limit is not exceeded

                busLogic.DebitGl(config.SavingsInterestExpenseGl, todaysInterest);
                busLogic.CreditGl(config.SavingsInterestPayableGl, todaysInterest);

                db.Entry(account).State = EntityState.Modified;
                db.Entry(config.SavingsInterestExpenseGl).State = EntityState.Modified;
                db.Entry(config.SavingsInterestPayableGl).State = EntityState.Modified;

                db.SaveChanges();

                frLogic.CreateTransaction(config.SavingsInterestExpenseGl, todaysInterest, TransactionType.Debit);
                frLogic.CreateTransaction(config.SavingsInterestPayableGl, todaysInterest, TransactionType.Credit);
            }//end foreach

                //monthly savings interest payment
                if (today.Day == daysInMonth[presentMonth - 1])     // checks for month end to calculate the interest and pay it into the appropriate account.
                {
                    bool customerIsCredited = false;
                    foreach (var account in allSavings)
                    {
                        busLogic.DebitGl(config.SavingsInterestPayableGl, account.dailyInterestAccrued);

                        //if the Withdrawal limit is not exceeded
                        if (!(account.SavingsWithdrawalCount > 3))    //assuming the withdrawal limit is 3
                        {
                            //Credit the customer ammount
                            busLogic.CreditCustomerAccount(account, account.dailyInterestAccrued);
                            customerIsCredited = true;
                        }
                        else
                        {
                            busLogic.CreditGl(config.SavingsInterestExpenseGl, account.dailyInterestAccrued);
                        }
                        account.SavingsWithdrawalCount = 0;  //re-initialize to zero for the next month
                        account.dailyInterestAccrued = 0;

                        db.Entry(account).State = EntityState.Modified;
                        db.Entry(config.SavingsInterestExpenseGl).State = EntityState.Modified;
                        db.Entry(config.SavingsInterestPayableGl).State = EntityState.Modified;

                        db.SaveChanges();

                        frLogic.CreateTransaction(config.SavingsInterestPayableGl, account.dailyInterestAccrued, TransactionType.Debit);
                        if (customerIsCredited)
                        {
                            frLogic.CreateTransaction(account, account.dailyInterestAccrued, TransactionType.Credit);
                        }
                        frLogic.CreateTransaction(config.SavingsInterestExpenseGl, account.dailyInterestAccrued, TransactionType.Credit);                                                
                    }
                }//end if
                //configRepo.Update(config);
            }
        }//end method ComputeAllSavingsInterestAccrued

        public void ComputeCurrentInterestAccrued()
        {
            //ideally current account don't get any interest but due to the context given, it is assumed they are given interest.
            int presentDay = today.Day;     //1 to totalDays in d month
            int daysRemaining = 0;
            if (custActRepo.AnyAccountOfType(AccountType.Current))
            {
                //note that COT is calculated upon withdarawal in the teller posting logic
                //the accrued COT is then deducted at EOD
                int presentMonth = today.Month;     //1 to 12

                #region Monthly COT deduction
                //monthly COT deduction
                /* if (today.Day == daysInMonth[presentMonth - 1])     //MONTH END?
                {
                    //var allCurrents = custActRepo.GetByType(AccountType.Current);
                    var allCurrents = db.CustomerAccounts.Where(a => a.AccountType == AccountType.Current).ToList();

                    foreach (var currentAccount in allCurrents)
                    {
                        ////Debit customer account
                        ////currentAccount.AccountBalance -= currentAccount.dailyCOTAccrued;   //accrued COT
                        busLogic.DebitCustomerAccount(currentAccount, currentAccount.dailyCOTAccrued);
                        busLogic.CreditGl(config.CurrentCotIncomeGl, currentAccount.dailyCOTAccrued);

                        currentAccount.dailyCOTAccrued = 0;    //goes back to zero after monthly deduction

                        db.Entry(currentAccount).State = EntityState.Modified;
                        db.Entry(config.CurrentCotIncomeGl).State = EntityState.Modified;

                        db.SaveChanges();

                        frLogic.CreateTransaction(currentAccount, currentAccount.dailyCOTAccrued, TransactionType.Debit);
                        frLogic.CreateTransaction(config.CurrentCotIncomeGl, currentAccount.dailyCOTAccrued, TransactionType.Credit);
                    }
                    //configRepo.Update(config);
                }//end if */
                #endregion

                // Daily COT deduction
                //var allCurrents = custActRepo.GetByType(AccountType.Current);
                var allCurrents = db.CustomerAccounts.Where(a => a.AccountType == AccountType.Current).ToList();

                foreach (var currentAccount in allCurrents)
                {

                    #region COT

                    busLogic.DebitCustomerAccount(currentAccount, currentAccount.dailyCOTAccrued);
                    busLogic.CreditGl(config.CurrentCotIncomeGl, currentAccount.dailyCOTAccrued);

                    currentAccount.dailyCOTAccrued = 0;    //goes back to zero after daily deduction

                    db.Entry(config.CurrentCotIncomeGl).State = EntityState.Modified;

                    frLogic.CreateTransaction(currentAccount, currentAccount.dailyCOTAccrued, TransactionType.Debit);
                    frLogic.CreateTransaction(config.CurrentCotIncomeGl, currentAccount.dailyCOTAccrued, TransactionType.Credit);

                    #endregion


                    //get the no of days remaining in this month
                    daysRemaining = daysInMonth[presentMonth - 1] - presentDay + 1;     //+1 because we havent computed for today
                    decimal interestRemainingForTheMonth = currentAccount.AccountBalance * (decimal)config.CurrentCreditInterestRate * daysRemaining / daysInMonth[presentMonth - 1];      //using I = PRT, where R is the rate per month
                    //calculate today's interest and add it to the account's dailyInterestAccrued
                    decimal todaysInterest = interestRemainingForTheMonth / daysRemaining;
                    currentAccount.dailyInterestAccrued += todaysInterest;     //increments till month end. Disbursed if withdrawal limit is not exceeded

                    busLogic.DebitGl(config.CurrentInterestExpenseGl, todaysInterest);
                    busLogic.CreditGl(config.CurrentInterestPayableGl, todaysInterest);

                    db.Entry(currentAccount).State = EntityState.Modified;
                    db.Entry(config.CurrentInterestExpenseGl).State = EntityState.Modified;
                    db.Entry(config.CurrentInterestPayableGl).State = EntityState.Modified;

                    db.SaveChanges();

                    frLogic.CreateTransaction(config.CurrentInterestExpenseGl, todaysInterest, TransactionType.Debit);
                    frLogic.CreateTransaction(config.CurrentInterestPayableGl, todaysInterest, TransactionType.Credit);
                }//end foreach

                //monthly current interest payment
                if (today.Day == daysInMonth[presentMonth - 1])     //MONTH END?
                {
                    bool customerIsCredited = false;
                    foreach (var account in allCurrents)
                    {
                        busLogic.DebitGl(config.CurrentInterestPayableGl, account.dailyInterestAccrued);

                        //if the Withdrawal limit is not exceeded
                        if (!(account.SavingsWithdrawalCount > 3))    //assuming the withdrawal limit is 3 and not more
                        {
                            //Credit the customer ammount
                            busLogic.CreditCustomerAccount(account, account.dailyInterestAccrued);
                            customerIsCredited = true;
                        }
                        else
                        {
                            busLogic.CreditGl(config.CurrentInterestExpenseGl, account.dailyInterestAccrued);
                        }
                        account.SavingsWithdrawalCount = 0;  //re-initialize to zero for the next month
                        account.dailyInterestAccrued = 0;

                        db.Entry(account).State = EntityState.Modified;
                        db.Entry(config.CurrentInterestExpenseGl).State = EntityState.Modified;
                        db.Entry(config.CurrentInterestPayableGl).State = EntityState.Modified;

                        db.SaveChanges();

                        frLogic.CreateTransaction(config.CurrentInterestPayableGl, account.dailyInterestAccrued, TransactionType.Debit);
                        if (customerIsCredited)
                        {
                            frLogic.CreateTransaction(account, account.dailyInterestAccrued, TransactionType.Credit);
                        }
                        frLogic.CreateTransaction(config.CurrentInterestExpenseGl, account.dailyInterestAccrued, TransactionType.Credit);
                    }
                }//end if
            }
   
        }//end current compute

        public void ComputeLoanInterestAccrued()
        {
            int presentMonth = today.Month;     //1 to 12
            decimal dailyInterestRepay = 0;

            if (custActRepo.AnyAccountOfType(AccountType.Loan))
            {
                //var allLoans = custActRepo.GetByType(AccountType.Loan);
                var allLoans = db.CustomerAccounts.Where(a => a.AccountType == AccountType.Loan).ToList();
                //daily loan repay
                foreach (var loanAccount in allLoans)
                {
                    dailyInterestRepay = loanAccount.LoanMonthlyInterestRepay / 30;     //assume 30 days in a month
                    loanAccount.dailyInterestAccrued += dailyInterestRepay;

                    busLogic.DebitGl(config.LoanInterestReceivableGl, dailyInterestRepay);
                    busLogic.CreditGl(config.LoanInterestIncomeGl, dailyInterestRepay);

                    loanAccount.DaysCount++;  //increments the day account was created after every EOD is successfully run

                    db.Entry(loanAccount).State = EntityState.Modified;
                    db.Entry(config.LoanInterestReceivableGl).State = EntityState.Modified;
                    db.Entry(config.LoanInterestIncomeGl).State = EntityState.Modified;

                    db.SaveChanges();

                    frLogic.CreateTransaction(config.LoanInterestReceivableGl, dailyInterestRepay, TransactionType.Debit);
                    frLogic.CreateTransaction(config.LoanInterestIncomeGl, dailyInterestRepay, TransactionType.Credit);
                }//end foreach

                //monthly loan deduction
                foreach (var loanAccount in allLoans)
                {
                    if (loanAccount.DaysCount % 30 == 0)      //checks monthly (30 days) cycle. the % is the remainder function used to check what is left
                    {
                        //pay back interest
                        busLogic.CreditGl(config.LoanInterestReceivableGl, loanAccount.dailyInterestAccrued);    //so the interestReceivable account balance goes back to zero                      
                        busLogic.DebitCustomerAccount(loanAccount.ServicingAccount, loanAccount.dailyInterestAccrued);

                        db.Entry(config.LoanInterestReceivableGl).State = EntityState.Modified;
                        db.Entry(loanAccount.ServicingAccount).State = EntityState.Modified;

                        db.SaveChanges();

                        frLogic.CreateTransaction(config.LoanInterestReceivableGl, loanAccount.dailyInterestAccrued, TransactionType.Credit);
                        frLogic.CreateTransaction(loanAccount.ServicingAccount, loanAccount.dailyInterestAccrued, TransactionType.Debit);

                        loanAccount.dailyInterestAccrued = 0;       //returns to zero after it has been debited
                        loanAccount.DaysCount = 0;
                        //pay back monthly principal
                        busLogic.CreditCustomerAccount(loanAccount, loanAccount.LoanMonthlyPrincipalRepay);
                        busLogic.DebitCustomerAccount(loanAccount.ServicingAccount, loanAccount.LoanMonthlyPrincipalRepay);

                        db.Entry(loanAccount).State = EntityState.Modified;
                        db.Entry(loanAccount.ServicingAccount).State = EntityState.Modified;

                        db.SaveChanges();

                        frLogic.CreateTransaction(loanAccount, loanAccount.LoanMonthlyPrincipalRepay, TransactionType.Credit);
                        frLogic.CreateTransaction(loanAccount.ServicingAccount, loanAccount.LoanMonthlyPrincipalRepay, TransactionType.Debit);

                        if (loanAccount.TermsOfLoan == TermsOfLoan.Reducing)        //the monthly paymment will change
                        {
                            loanAccount.LoanMonthlyInterestRepay = loanAccount.LoanInterestRatePerMonth * loanAccount.LoanPrincipalRemaining;
                            loanAccount.LoanMonthlyPrincipalRepay = loanAccount.LoanMonthlyRepay - loanAccount.LoanMonthlyInterestRepay;
                            loanAccount.LoanPrincipalRemaining = loanAccount.LoanMonthlyPrincipalRepay;
                        }

                        db.Entry(loanAccount).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                //db.SaveChanges();
            }//end if         
        }//end method ComputeDailyLoanInterestAccrued

        public void SaveDailyIncomeAndExpenseBalance()
        {
            var allIncomes = glRepo.GetByMainCategory(MainGlCategory.Income);
            //save daily balance of all income acccounts
            foreach (var account in allIncomes)
            {
                var entry = new ExpenseIncomeEntry();
                entry.AccountName = account.AccountName;
                entry.Amount = account.AccountBalance;
                entry.Date = today;
                entry.EntryType = PandLType.Income;
                db.ExpenseIncomeEntries.Add(entry);
                db.SaveChanges();
                //new ProfitAndLossRepository().Insert(entry);
            }
            //save daily balance off all expense accounts
            var allExpenses = glRepo.GetByMainCategory(MainGlCategory.Expenses);
            foreach (var account in allExpenses)
            {
                var entry = new ExpenseIncomeEntry();
                entry.AccountName = account.AccountName;
                entry.Amount = account.AccountBalance;
                entry.Date = today;
                entry.EntryType = PandLType.Expenses;
                db.ExpenseIncomeEntries.Add(entry);
                db.SaveChanges(); ;
            }
        }
    }
}

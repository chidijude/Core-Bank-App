using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class ProfitAndLossRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        DateTime yesterday;
        public ProfitAndLossRepository()
        {
            yesterday = db.AccountConfigurations.First().FinancialDate.AddDays(-1); //since EOD has not been run today there is no Expense/Income entries for today until EOD is run.
        }
        public List<ExpenseIncomeEntry> GetAllExpenseIncomeEntries()
        {
            return db.ExpenseIncomeEntries.ToList();
        }
        public List<ExpenseIncomeEntry> GetEntries()
        {
            var result = new List<ExpenseIncomeEntry>();
            var allEntries = GetAllExpenseIncomeEntries();
            foreach (var item in allEntries)
            {
                if (item.Date.Date == yesterday.Date)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ExpenseIncomeEntry> GetEntries(DateTime startDate, DateTime endDate)
        {
            var result = new List<ExpenseIncomeEntry>();
            if (startDate < endDate)
            {
                //gets all entries(with their balances) for the start and the end dates. 
                var allEntries = GetAllExpenseIncomeEntries();
                foreach (var item in allEntries)
                {
                    if (item.Date.Date == startDate || item.Date.Date == endDate)
                    {
                        result.Add(item);
                    }
                }

            }
            return result.OrderByDescending(e => e.Date).ToList();  //making entries on endDate to come before those of startDate so that the difference in Account balance between the two days could be easily calculated
        }
    }
}
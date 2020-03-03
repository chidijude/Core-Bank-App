using Express.Core.Models;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUCBA.Logic
{
    public class ProfitAndLossLogic
    {
        ProfitAndLossRepository plRepo = new ProfitAndLossRepository();

        public List<ExpenseIncomeEntry> GetEntries()
        {
            return plRepo.GetEntries();
        }

        public List<ExpenseIncomeEntry> GetEntries(DateTime startDate, DateTime endDate)
        {
            return plRepo.GetEntries(startDate, endDate);
        }
    }
}

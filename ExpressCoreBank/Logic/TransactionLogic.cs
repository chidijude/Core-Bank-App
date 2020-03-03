using Express.Core.Models;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUCBA.Logic
{
    public class TransactionLogic
    {
        TransactionRepository transRepo = new TransactionRepository();

        public List<Transaction> GetTrialBalanceTransactions(DateTime startDate, DateTime endDate)
        {
            return transRepo.GetTrialBalanceTransactions(startDate, endDate);
        }
    }
}

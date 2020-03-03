using Express.Core.Models;
using Express.Core.ViewModels.FinancialReportViewModel;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public class BalanceSheetLogic
    {
        BalanceSheetRepository bsRepo = new BalanceSheetRepository();

        public List<GlAccount> GetAssetAccounts()
        {
            return bsRepo.GetAssetAccounts();
        }

        public List<GlAccount> GetCapitalAccounts()
        {
            return bsRepo.GetCapitalAccounts();
        }

        public List<GlAccount> GetIncomeAccounts()
        {
            return bsRepo.GetIncomeAccounts();
        }

        public List<GlAccount> GetExpenseAccounts()
        {
            return bsRepo.GetExpenseAccounts();
        }
        public List<LiabilityViewModel> GetLiabilityAccounts()
        {
            return bsRepo.GetLiabilityAccounts();
        }
    }
}

using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class ConfigurationRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public void Initialize()
        {
            if (db.AccountConfigurations.Count() < 1)    //no configuration yet
            {
                db.AccountConfigurations.Add(new AccountConfiguration() { FinancialDate = DateTime.Now, IsBusinessOpen = false });
                db.SaveChanges();
            }
        }

        public AccountConfiguration GetFirst()
        {
            return db.AccountConfigurations.First();
        }
    }
}
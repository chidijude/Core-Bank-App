using Express.Core.Models;
using ExpressCoreBank.Models;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public class TellerMgtLogic
    {
        TellerMgtRepository tellerMgtRepo = new TellerMgtRepository();
        UserRepository userRepo = new UserRepository();
        GlAccountRepository glActRepo = new GlAccountRepository();
        
        public List<UserTill> ExtractAllTellerInfo()
        {
            var output = new List<UserTill>();
            var tellersWithTill = tellerMgtRepo.GetAll();
            var tellersWithoutTill = userRepo.TellersWithoutTill();

            //adding all tellers without a till account 
            foreach (var teller in tellersWithoutTill)
            {
                output.Add(new UserTill { UserId = teller.Id, GlAccountID = 0 });
            }
            //adding all tellers with a till account
            output.AddRange(tellersWithTill);
            return output;
        }

        public List<User> ExtractTellersWithoutTill()
        {
            return userRepo.TellersWithoutTill();
        }

        public List<GlAccount> ExtractTillsWithoutTeller()
        {
            return glActRepo.TillsWithoutTeller();
        }
    }
}

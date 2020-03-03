using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class GlAccountRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool AnyGlIn(MainGlCategory mainCategory)
        {
            return db.GlAccounts.Any(gl => gl.GlCategory.MainCategory == mainCategory);
        }
        public GlAccount GetLastGlIn(MainGlCategory mainCategory)
        {
            return db.GlAccounts.Where(g => g.GlCategory.MainCategory == mainCategory).OrderByDescending(a => a.ID).First();
        }

        public GlAccount GetByName(string name)
        {
            return db.GlAccounts.Where(a => a.AccountName.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }

        public List<GlAccount> GetAllTills()
        {
            return db.GlAccounts.Where(a => a.AccountName.ToLower().Contains("till") && a.GlCategory.MainCategory == MainGlCategory.Asset).ToList();
        }

        public List<GlAccount> TillsWithoutTeller()
        {
            var tills = GetAllTills();

            var output = new List<GlAccount>();
            var tillToUsers = db.TillUsers.ToList();

            foreach (var till in tills)
            {
                if (!tillToUsers.Any(tu => tu.GlAccountID == till.ID))
                {
                    output.Add(till);
                }
            }
            return output;
        }

        public GlAccount GetUserTill(User teller)
        {
            bool tellerHasTill = db.TillUsers.Any(tu => tu.UserId == teller.Id);

            if (tellerHasTill)
            {
                int tillId = db.TillUsers.Where(tu => tu.UserId == teller.Id).First().GlAccountID;
                return db.GlAccounts.Find(tillId);
            }
            return null;
        }

        public List<GlAccount> GetByMainCategory(MainGlCategory mainCategory)
        {
            return db.GlAccounts.Where(a => a.GlCategory.MainCategory == mainCategory).ToList();
        }
    }
}
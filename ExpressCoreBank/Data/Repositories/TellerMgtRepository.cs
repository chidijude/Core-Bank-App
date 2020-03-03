using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class TellerMgtRepository
    {
        private  ApplicationDbContext db = new ApplicationDbContext();

        public List<UserTill> GetAll()
        {
            return db.TillUsers.ToList();
        }
    }
}
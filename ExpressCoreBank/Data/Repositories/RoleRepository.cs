using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class RoleRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Role GetByName(string name)
        {
            return db.Roles.Where(r => r.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }
    }
}

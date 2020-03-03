using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class BranchRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Branch GetByName(string name)
        {
            return db.Branches.Where(b => b.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }

        public List<Branch> GetAll()
        {
            return db.Branches.ToList();
        }
    }
}
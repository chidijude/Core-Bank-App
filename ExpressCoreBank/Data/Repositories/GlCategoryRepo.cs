using Express.Core.Models;
using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class GlCategoryRepo
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public GlCategory GetByName(string name)
        {
            return db.GlCategories.Where(c => c.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }
    }
}
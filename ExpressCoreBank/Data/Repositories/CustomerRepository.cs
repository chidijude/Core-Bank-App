using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class CustomerRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public Customer GetById(int custId)
        {
            return db.Customers.FirstOrDefault(c => c.ID == custId); //.Where(c => c.CustId).ToLower().Equals(custId.ToLower())).FirstOrDefault();
        }

        public List<Customer> GetAll()
        {
            return db.Customers.ToList();
        }
    }
}
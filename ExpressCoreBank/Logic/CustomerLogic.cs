using ExpressCoreBank.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public class CustomerLogic
    {
        CustomerRepository custRepo = new CustomerRepository();
        public string GenerateCustomerId()
        {
            //get the Id of the last customer

            var customers = custRepo.GetAll().OrderByDescending(c => c.ID);

            string id = "00000001";         //assume no customer initially
            if (customers != null && customers.Count() > 0)
            {
                long lastId = Convert.ToInt64(customers.First().ID);
                id = (lastId + 1).ToString("D8");       //customer id of the last customer + 1
            }
            return id;
        }
    }
}

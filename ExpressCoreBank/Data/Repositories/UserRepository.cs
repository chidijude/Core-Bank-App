using ExpressCoreBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      

        public ApplicationUser GetByUsername(string username)
        {
            return db.Users.Where(u => u.UserName.ToLower().Equals(username.ToLower())).FirstOrDefault();
        }

        public ApplicationUser GetByEmail(string email)
        {
            return db.Users.Where(u => u.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();
        }

        
        public ApplicationUser FindUser(string username, string passwordHash)
        {
            return db.Users.Where(u => u.UserName.ToLower().Equals(username.ToLower()) && u.PasswordHash.ToLower().Equals(passwordHash.ToLower())).FirstOrDefault();
        }

        public object GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public object GetAllUsersByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllTellers()
        {
            return db.Userrs.ToList(); //.Where(u => u.Role.RoleClaims.Any(r => r.Name.Equals("TellerPosting"))).ToList();
        }

        public List<User> TellersWithoutTill()
        {
            var tellers = GetAllTellers();
            var output = new List<User>();

            var tillToUsers = db.TillUsers.ToList();
            foreach (var teller in tellers)
            {
                if (!tillToUsers.Any(tu => tu.UserId == teller.Id))
                {
                    output.Add(teller);
                }
            }
            return output;
        }

        public List<User> TellersWithTill()
        {
            var tellers = GetAllTellers();
            var output = new List<User>();
            var tillToUsers = db.TillUsers.ToList();
            foreach (var teller in tellers)
            {
                if (tillToUsers.Any(tu => tu.UserId == teller.Id))
                {
                    output.Add(teller);
                }
            }
            return output;
        }

        public List<User> GetAllWithEmailConfirmed()
        {
            return db.Userrs.Where(u => u.EmailConfirmed == true).ToList();
        }
    }
}
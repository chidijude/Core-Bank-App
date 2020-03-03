using System.Data.Entity;
using Express.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpressCoreBank.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<User> Userrs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<GlCategory> GlCategories { get; set; }
        public DbSet<GlAccount> GlAccounts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<UserTill> TillUsers { get; set; }
        public DbSet<AccountConfiguration> AccountConfigurations { get; set; }
        public DbSet<GlPosting> GlPostings { get; set; }
        public DbSet<TellerPosting> TellerPostings { get; set; }
        public DbSet<ExpenseIncomeEntry> ExpenseIncomeEntries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

       
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Express.Core.ViewModels.TillToUserViewModel> TillToUserViewModels { get; set; }
    }
}
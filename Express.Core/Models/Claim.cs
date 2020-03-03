using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    class Claim
    {
        public static class ClaimData
        {
            public static List<string> AppClaims { get; set; } = new List<string>
        {
             "BranchMgt", "UserMgt", "TellerMgt", "CustomerMgt", "CustomerAcccountMgt", "GLPosting", "TellerPosting", "AccountConfigMgt", "PostingAuth", "RunEOD", "RoleMgt", "GLMgt", "FinancialReport"
        };

            public static string ClaimType = "DynamicClaim";
            //    public int ID { get; set; }
            //    public string Name { get; set; }

            //    public int RoleID { get; set; }
            //    public virtual Role Role { get; set; }
        }
    }
}
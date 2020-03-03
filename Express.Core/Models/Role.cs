using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public class Role
    {
       
            public int ID { get; set; }

            //[Required]
            //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Role name should only contain characters with no white space"), MaxLength(40)]
            public string Name { get; set; }

            public virtual ICollection<RoleClaim> RoleClaims { get; set; }
 
    }
}

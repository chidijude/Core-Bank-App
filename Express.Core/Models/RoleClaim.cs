using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Express.Core.Models
{
    public class RoleClaim
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}

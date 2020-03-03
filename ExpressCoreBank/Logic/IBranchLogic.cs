using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public interface IBranchLogic
    {
        long GetSortCode();
        bool IsBranchNameExists(string name);
    }
}

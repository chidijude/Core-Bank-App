using Express.Core.Models;
using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressLogic
{
    public class GlAccountLogic
    {
        GlAccountRepository glRepo = new GlAccountRepository();
        public long GenerateGLAccountNumber(MainGlCategory glMainCategory)
        {
            long code = 0;

            //get the last account number in this category
            if (glRepo.AnyGlIn(glMainCategory))
            {
                var lastAct = glRepo.GetLastGlIn(glMainCategory);
                code = lastAct.CodeNumber + 1;
            }

            else                
            {
                switch (glMainCategory)     //the codes below are just used at my discretion.
                {
                    case MainGlCategory.Asset:
                        code = 1000102016;
                        break;
                    case MainGlCategory.Capital:
                        code = 3000102016;
                        break;
                    case MainGlCategory.Expenses:
                        code = 5000102016;
                        break;
                    case MainGlCategory.Income:
                        code = 4000102016;
                        break;
                    case MainGlCategory.Liability:
                        code = 2000102016;
                        break;
                    default:
                        break;
                }
            }//end if

            return code;
        }

        public bool IsUniqueName(string name)
        {
            if (glRepo.GetByName(name) == null)
            {
                return true;
            }
            return false;
        }
    }
}

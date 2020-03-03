using ExpressCoreBank.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressCoreBank.Logic
{
    public class RoleLogic
    {
                
            RoleRepository roleRepo = new RoleRepository();
            public bool isRoleNameExists(string name)
            {
                if (roleRepo.GetByName(name) == null)
                {
                    return false;
                }
                return true;
            }
        
    }
}
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.Helper
{

    public static class SecurityGroupDTO
    {
        private static MIS_DBEntities1 db = new MIS_DBEntities1();

        public static bool CheckSuperAdmin(int groupId)
        {
            bool isSuperAdmin = false;
            var getSuperAdmin = db.SecurityGroups.Where(x => x.Parent_Id == 0 && x.Id == groupId).SingleOrDefault();
            if (getSuperAdmin != null)
            {
                isSuperAdmin = true;
            }
            else
            {
                isSuperAdmin = false;
            }
            return isSuperAdmin;
        }
    }
}
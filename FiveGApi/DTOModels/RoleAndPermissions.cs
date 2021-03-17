using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class RoleAndPermissions
    {
        public  int? RoleID { get; set; }
        public string Role { get; set; }
        public List<int> RolePermisions { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class FormDTO
    {
        public string url { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool title { get; set; }
    }
    public class PermissionFormDTO
    {
        public string name { get; set; }
        public int ModuleID { get; set; }
        public List<FormPermission> ModuleFormsList { get; set; }
    }
    public class FormPermission
    {
        public int FormID { get; set; }
        public string  FormName { get; set; }
    }
}
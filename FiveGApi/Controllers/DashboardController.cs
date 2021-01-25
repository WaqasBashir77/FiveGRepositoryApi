using FiveGApi.DTOModels;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        //private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        [HttpPost]
        [Route("getmenu")]
        public IHttpActionResult GetMenu(GeneralDTO general)
        {
            RolesToUser getUserRole = db.RolesToUsers.Where(x => x.UserId == general.UserId).FirstOrDefault();
            List<FormDTO> forms = new List<FormDTO>();
            if (getUserRole != null)
            {
                var getAllActiveForm = db.PrivilegesToRoles.Where(x => x.RoleId == getUserRole.RoleId && x.Status == true).OrderBy(x=> x.FormOrder).ToList();

                foreach (var item in getAllActiveForm.ToList())
                {
                    var getMenuItem = db.Forms.Where(x => x.Id == item.FormId).SingleOrDefault();
                    var getModuleItem = db.Modules.Where(x => x.Id == getMenuItem.ModuleId).SingleOrDefault();
                  
                   
                    if (!forms.Any(x=> x.name == getModuleItem.Name))
                    {
                        FormDTO moduleDTO = new FormDTO();
                        moduleDTO.name = getModuleItem.Name;
                        moduleDTO.title = true;
                        forms.Add(moduleDTO);
                    }
                    FormDTO formDTO = new FormDTO();
                    formDTO.name = getMenuItem.Alias;
                    formDTO.icon = getMenuItem.Icon;
                    formDTO.url = getMenuItem.FormUrl;

                    forms.Add(formDTO);
                }
            }

            return Ok(forms);
        }
    }
}

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
            RolesToUser getUserRole = db.RolesToUsers.Where(x => x.UserId == general.UserId).AsQueryable().FirstOrDefault();
            List<FormDTO> forms = new List<FormDTO>();
            if (getUserRole != null)
            {
                var pra = (from f in db.Forms
                          join p in db.PrivilegesToRoles on f.Id equals p.FormId
                          where p.RoleId == getUserRole.RoleId
                          select f).OrderBy(x=>x.OrderBy).GroupBy(x=>x.ModuleId).AsQueryable().ToList();
                if (pra.Count() < 0)
                {
                    return NotFound();
                }
                else
                {
                    var modulesList=db.Modules.AsQueryable().ToList();
                    foreach (var item in pra)
                    {
                        foreach (var Formvalues in item)
                        {
                            var getMenuItem = Formvalues;
                            var getModuleItem = modulesList.Where(x => x.Id == getMenuItem.ModuleId).FirstOrDefault();
                            if (!forms.Any(x => x.name == getModuleItem.Name))
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
                }
            }
            return Ok(forms);
        }
    }
}

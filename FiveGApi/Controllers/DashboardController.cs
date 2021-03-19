using FiveGApi.DTOModels;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        //private FiveG_DBEntities db = new FiveG_DBEntities();
        private MIS_DBEntities1 db = new MIS_DBEntities1();
        private string UserId;
        public DashboardController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
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
                          && f.IsMenuItem==true
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
        
        [Route("GetBookingData")]
        public IHttpActionResult GetBookingData()
        {
            var BookingConfirms = db.BookingConfirms.ToList();
           var totalBooking= BookingConfirms.Count();
           var totalAuthorizedBooking = BookingConfirms.Where(x=>x.Authorize_Status== "Authorized").AsQueryable().Count();
           var totalPendingBooking = BookingConfirms.Where(x=>x.Authorize_Status== "Pending").AsQueryable().Count();
           var totalDraftBooking = BookingConfirms.Where(x=>x.Authorize_Status== "Draft").AsQueryable().Count();
            decimal totalAuthorizedP = (decimal)(((decimal)totalAuthorizedBooking / (decimal)totalBooking) * 100);
            decimal totalPendingP = (decimal)((decimal)totalPendingBooking / (decimal)totalBooking) * 100;
            decimal totalDraftP = (decimal)((decimal)totalDraftBooking / (decimal)totalBooking) * 100;
            var totalPendingPayments = BookingConfirms.Where(x => x.Authorize_Status == "Pending").AsQueryable().Sum(x => x.Booking_amount);
            var totalAuthorizedPayments = BookingConfirms.Where(x => x.Authorize_Status == "Authorized").AsQueryable().Sum(x => x.Booking_amount);
            var totalDraftPayments = BookingConfirms.Where(x => x.Authorize_Status == "Draft").AsQueryable().Sum(x => x.Booking_amount);
            var list = new List<Tuple<string, string>>();
            list.Add(new Tuple<string, string>("totalBooking", totalBooking.ToString()));
            list.Add(new Tuple<string, string>("totalAuthorizedBooking", totalAuthorizedBooking.ToString()));
            list.Add(new Tuple<string, string>("totalPendingBooking", totalPendingBooking.ToString()));
            list.Add(new Tuple<string, string>("totalDraftBooking", totalDraftBooking.ToString()));
            list.Add(new Tuple<string, string>("totalAuthorizedP", totalAuthorizedP.ToString()));
            list.Add(new Tuple<string, string>("totalPendingP", totalPendingP.ToString()));
            list.Add(new Tuple<string, string>("totalDraftP", totalDraftP.ToString()));
            list.Add(new Tuple<string, string>("totalPendingPayments", totalPendingPayments.ToString()));
            list.Add(new Tuple<string, string>("totalAuthorizedPayments", totalAuthorizedPayments.ToString()));
            list.Add(new Tuple<string, string>("totalDraftPayments", totalDraftPayments.ToString()));
            return Ok(list);
        }
        [Route("GetSocietyReciveAblecommision")]
        public IHttpActionResult GetSocietyReciveAblecommision()
        {
            var pra = (from f in db.PropertyDefs
                       join p in db.BookingConfirms on f.ID equals p.Property_ID
                       select p).GroupBy(x=>x.Property_ID).AsQueryable().DefaultIfEmpty().ToList();
            if (pra == null)
            {
                return NotFound();
            }
            else
            {
                var PropertyDefs= db.PropertyDefs.ToList();
                List<PropertyDashboardModel> propertyDashboardModels = new List<PropertyDashboardModel>();
                var totalBookingconfirmSale = db.BookingConfirms.Sum(x => x.Total_amount);
                foreach (var item in pra)
                {
                    PropertyDashboardModel propertyDashboardModel = new PropertyDashboardModel();
                    var propertyID = Convert.ToInt32(item.Select(x => x.Property_ID).FirstOrDefault());
                    propertyDashboardModel.PropertyName = PropertyDefs.Where(x => x.ID == propertyID).Select(x => x.Name).FirstOrDefault();
                    propertyDashboardModel.PropertySaleVlaue = item.Sum(x => x.Total_amount);
                    propertyDashboardModel.PropertyTotalCount = item.Count();
                    propertyDashboardModel.TotalPercentage = (decimal)(((decimal)propertyDashboardModel.PropertySaleVlaue / (decimal)totalBookingconfirmSale) * 100);
                    propertyDashboardModels.Add(propertyDashboardModel);
                }
                return Ok(propertyDashboardModels);
            }
        }
        [Route("GetReceivableCommissionBySociety")]
        public IHttpActionResult GetReceivableCommissionBySociety()
        {
            var pra = (from f in db.PropertyDefs
                       join p in db.BookingConfirms on f.ID equals p.Property_ID
                       select p).GroupBy(x => x.Property_ID).AsQueryable().DefaultIfEmpty().ToList();
            if (pra == null)
            {
                return NotFound();
            }
            else
            {
                var PropertyDefs = db.PropertyDefs.ToList();
                List<CommisionReciveableDashboard> commisionReciveableDashboards = new List<CommisionReciveableDashboard>();
                //var totalBookingconfirmSale = db.BookingConfirms.Sum(x => x.Total_amount);
                foreach (var item in pra)
                {
                    CommisionReciveableDashboard propertyDashboardModel = new CommisionReciveableDashboard();
                    var propertyID = Convert.ToInt32(item.Select(x => x.Property_ID).FirstOrDefault());
                    propertyDashboardModel.SocietyName = PropertyDefs.Where(x => x.ID == propertyID).Select(x => x.Name).FirstOrDefault();
                   var listBookingPaid= item.Where(x => x.Payment_B_Status == "paid").ToList();
                    propertyDashboardModel.PropertyBooking = item.Where(x=>x.Payment_B_Status=="Paid").Count();
                    propertyDashboardModel.BookingCommision = item.Where(x=>x.Payment_B_Status=="paid").Sum(x=>x.Booking_amount);
                    propertyDashboardModel.PropertyConfimr = item.Where(x => x.Payment_C_Status == "paid").Count();
                    propertyDashboardModel.ConfimrCommision = item.Where(x => x.Payment_C_Status == "paid").Sum(x => x.Confirm_amount);
                    propertyDashboardModel.TotalCommision = propertyDashboardModel.ConfimrCommision + propertyDashboardModel.PropertyConfimr;
                    commisionReciveableDashboards.Add(propertyDashboardModel);
                }
                return Ok(commisionReciveableDashboards);
            }
        }
    }
}

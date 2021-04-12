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
                           && f.IsMenuItem == true
                           select f).OrderBy(x => x.OrderBy).GroupBy(x => x.ModuleId).AsQueryable().ToList();
                if (pra.Count() < 0)
                {
                    return NotFound();
                }
                else
                {
                    var modulesList = db.Modules.AsQueryable().ToList();
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
            var totalBooking = BookingConfirms.Count();
            var totalAuthorizedBooking = BookingConfirms.Where(x => x.Authorize_Status == "Authorized").AsQueryable().Count();
            var totalPendingBooking = BookingConfirms.Where(x => x.Authorize_Status == "Pending").AsQueryable().Count();
            var totalDraftBooking = BookingConfirms.Where(x => x.Authorize_Status == "Draft").AsQueryable().Count();
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
                       select p).GroupBy(x => x.Property_ID).AsQueryable().DefaultIfEmpty().ToList();
            if (pra == null)
            {
                return NotFound();
            }
            else
            {
                var PropertyDefs = db.PropertyDefs.ToList();
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
                    var listBookingPaid = item.Where(x => x.Payment_B_Status == "paid").ToList();
                    propertyDashboardModel.PropertyBooking = item.Where(x => x.Payment_B_Status == "Paid").Count();
                    propertyDashboardModel.BookingCommision = item.Where(x => x.Payment_B_Status == "paid").Sum(x => x.Booking_amount);
                    propertyDashboardModel.PropertyConfimr = item.Where(x => x.Payment_C_Status == "paid").Count();
                    propertyDashboardModel.ConfimrCommision = item.Where(x => x.Payment_C_Status == "paid").Sum(x => x.Confirm_amount);
                    propertyDashboardModel.TotalCommision = propertyDashboardModel.ConfimrCommision + propertyDashboardModel.PropertyConfimr;
                    commisionReciveableDashboards.Add(propertyDashboardModel);
                }
                return Ok(commisionReciveableDashboards);
            }
        }
        [Route("GetProjectData")]
        public IHttpActionResult GetProjectData()
        {
            var ProjectsConfirms = db.Projects.Select(x => new ProjectListDTO
            {
                Id = x.Id,
                projectCode = x.projectCode,
                location = x.location,
                projectName = x.projectName,
                projectType = x.projectType,
                totalArea = x.totalArea,
                address = x.address,
                city = x.city,
                status = x.status,
                description = x.description,
                unit = x.unit,
                noc = x.noc,
                projectCurrency = x.projectCurrency,
                PaymentPlanStatus = x.PaymentPlanStatus,
                SecurityGroupId = x.SecurityGroupId,
                Created_By = x.Created_By,
                Created_Date = x.Created_Date,
                Update_By = x.Update_By,
                Update_Date = x.Update_Date,
                LocationSeg = x.location,
                Company = x.Company,
                ProjectSeg = x.ProjectSeg,
                PlotAres = x.PlotAres,
                ProjectDetails = db.ProjectDetails.Where(y => y.projectId == x.Id).ToList()
            }).AsQueryable().ToList();
            var totalProjects = ProjectsConfirms.Count();
            var totalUnderconstructionProjects = ProjectsConfirms.Where(x => x.status == "Underconstruction").AsQueryable().Count();
            var totalDevelopProjects = ProjectsConfirms.Where(x => x.status == "Develop").AsQueryable().Count();
            var ProjectSale = db.PropertySales.Select(x => new PropertySaleDashboardDto
            {
                AuthorizeStatus = x.AuthorizeStatus,
                Sale_Status = x.Sale_Status,
            }).ToList();
            var TotalProjectSale = ProjectSale.Count();
            var TotalProjectAuthorizedSale = ProjectSale.Where(x => x.AuthorizeStatus == true).AsQueryable().Count();
            var TotalProjectUnAuthorizedSale = ProjectSale.Where(x => x.AuthorizeStatus == false).AsQueryable().Count();
            var TotalProjectProjectsBySaleStatus = ProjectSale.Where(x => x.Sale_Status == "Amendment").AsQueryable().Count();
            var TotalProjectNewSale = ProjectSale.Where(x => x.Sale_Status == "New").AsQueryable().Count();
            var TotalProjectCancelSale = ProjectSale.Where(x => x.Sale_Status == "Cancel").AsQueryable().Count();
            var SaleInstallments = db.SaleInstallments.ToList();
            var TotalPaidProjectSaleBookings = SaleInstallments.Where(x => x.ins_milestone == "Booking" && x.ins_payment_status == "Paid").AsQueryable().Count();
            var TotalPaidProjectSaleConfirmations = SaleInstallments.Where(x => x.ins_milestone == "Confirmation" && x.ins_payment_status == "Paid").AsQueryable().Count();
            var TotalPaidProjectSalePossessions = SaleInstallments.Where(x => x.ins_milestone == "Possession" && x.ins_payment_status == "Paid").AsQueryable().Count();

            var Total_Amendment_ProjectSale = ProjectSale.Where(x => x.Sale_Status == "Amendment").AsQueryable().Count();
            var Total_New_ProjectSale = ProjectSale.Where(x => x.Sale_Status == "New").AsQueryable().Count();
            var Total_Cancel_ProjectSalePayments = ProjectSale.Where(x => x.Sale_Status == "Cancel").AsQueryable().Count();


            var Total_Amendment_Amount_ProjectSale = (from ps in db.PropertySales
                                                      join si in db.SaleInstallments on ps.Booking_ID equals si.Booking_ID
                                                      where si.ins_payment_status == "Paid" && ps.Sale_Status == "Amendment"
                                                      select si).AsQueryable().Sum(x => x.ins_amount);
            var Total_Cancel_Amount_ProjectSale = (from ps in db.PropertySales
                                                   join si in db.SaleInstallments on ps.Booking_ID equals si.Booking_ID
                                                   where si.ins_payment_status == "Paid" && ps.Sale_Status == "Cancel"
                                                   select si).AsQueryable().Sum(x => x.ins_amount);
            var Total_New_Amount_ProjectSale = (from ps in db.PropertySales
                                                join si in db.SaleInstallments on ps.Booking_ID equals si.Booking_ID
                                                where si.ins_payment_status == "Paid" && ps.Sale_Status == "New"
                                                select si).AsQueryable().Sum(x => x.ins_amount);

            //var TotalPaidProjectSalePossessions = SaleInstallments.Where(x => x.ins_milestone == "Possession" && x.ins_payment_status == "Paid").AsQueryable().Count();

            var list = new List<Tuple<string, string>>();
            list.Add(new Tuple<string, string>("totalProjects", totalProjects.ToString()));
            list.Add(new Tuple<string, string>("totalUnderconstructionProjects", totalUnderconstructionProjects.ToString()));
            list.Add(new Tuple<string, string>("totalDevelopProjects", totalDevelopProjects.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectSale", TotalProjectSale.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectAuthorizedSale", TotalProjectAuthorizedSale.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectUnAuthorizedSale", TotalProjectUnAuthorizedSale.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectProjectsBySaleStatus", TotalProjectProjectsBySaleStatus.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectNewSale", TotalProjectNewSale.ToString()));
            list.Add(new Tuple<string, string>("TotalProjectCancelSale", TotalProjectCancelSale.ToString()));
            list.Add(new Tuple<string, string>("TotalPaidProjectSaleBookings", TotalPaidProjectSaleBookings.ToString()));
            list.Add(new Tuple<string, string>("TotalPaidProjectSaleConfirmations", TotalPaidProjectSaleConfirmations.ToString()));
            list.Add(new Tuple<string, string>("TotalPaidProjectSalePossessions", TotalPaidProjectSalePossessions.ToString()));
            list.Add(new Tuple<string, string>("Total_Amendment_ProjectSale", Total_Amendment_Amount_ProjectSale.ToString()));
            list.Add(new Tuple<string, string>("Total_New_ProjectSale", Total_New_ProjectSale.ToString()));
            list.Add(new Tuple<string, string>("Total_Cancel_ProjectSalePayments", Total_Cancel_ProjectSalePayments.ToString()));
            list.Add(new Tuple<string, string>("Total_Amendment_Amount_ProjectSale", Total_Amendment_Amount_ProjectSale.ToString()));
            list.Add(new Tuple<string, string>("Total_Cancel_Amount_ProjectSale", Total_Cancel_Amount_ProjectSale.ToString()));
            list.Add(new Tuple<string, string>("Total_New_Amount_ProjectSale", Total_New_Amount_ProjectSale.ToString()));
            return Ok(list);
        }
        [Route("GetCommisionData")]
        public IHttpActionResult GetCommisionData()
        {
            var EmployeeList = db.Registrations.Where(x => x.Type == "Staff").ToList();
            var DealerList = db.Registrations.Where(x => x.Type == "Dealer").ToList();
            var EmployeeCommisionList = new List<EmployeeCommisionList>();
            var DealerCommisionList = new List<EmployeeCommisionList>();

            if (EmployeeList != null)
            {
                foreach (var eList in EmployeeList)
                {
                    EmployeeCommisionList employeeCommisionList = new EmployeeCommisionList();
                    employeeCommisionList.Name = eList.StaffName;
                    employeeCommisionList.totalBookingCommisson = db.BookingConfirms.Where(x => x.Book_Emp == eList.ID.ToString()).AsQueryable().Sum(x => x.Emp_B_RAmt);
                    employeeCommisionList.totalConfirmCommisson = db.BookingConfirms.Where(x => x.Book_Emp == eList.ID.ToString()).AsQueryable().Sum(x => x.Emp_C_RAmt);
                    employeeCommisionList.totalCommisson = employeeCommisionList.totalBookingCommisson + employeeCommisionList.totalConfirmCommisson;
                    employeeCommisionList.totalNoBookings = db.BookingConfirms.Where(x => x.Book_Emp == eList.ID.ToString() && x.Emp_B_RAmt>0).AsQueryable().Count();
                    employeeCommisionList.totalNoCommision = db.BookingConfirms.Where(x => x.Book_Emp == eList.ID.ToString() && x.Emp_C_RAmt > 0).AsQueryable().Count();

                    EmployeeCommisionList.Add(employeeCommisionList);
                }
            }
            if (DealerList != null)
            {
                foreach (var eList in DealerList)
                {
                    EmployeeCommisionList dCommisionList = new EmployeeCommisionList();
                    dCommisionList.Name = eList.Name;
                    dCommisionList.totalBookingCommisson = db.BookingConfirms.Where(x => x.Book_Dealer == eList.ID.ToString()).AsQueryable().Sum(x => x.Dealer_B_RAmt);
                    dCommisionList.totalConfirmCommisson = db.BookingConfirms.Where(x => x.Book_Dealer == eList.ID.ToString()).AsQueryable().Sum(x => x.Dealer_C_RAmt);
                    dCommisionList.totalCommisson = dCommisionList.totalBookingCommisson + dCommisionList.totalConfirmCommisson;
                    dCommisionList.totalNoBookings = db.BookingConfirms.Where(x => x.Book_Dealer == eList.ID.ToString() && x.Dealer_B_RAmt>0).AsQueryable().Count();
                    dCommisionList.totalNoCommision = db.BookingConfirms.Where(x => x.Book_Dealer == eList.ID.ToString() && x.Dealer_C_RAmt>0).AsQueryable().Count();
                    DealerCommisionList.Add(dCommisionList);
                }
            }
            var list = new List<Tuple<string, List<EmployeeCommisionList>>>();
            list.Add(new Tuple<string, List<EmployeeCommisionList>>("EmployeeCommisionList", EmployeeCommisionList));
            list.Add(new Tuple<string, List<EmployeeCommisionList>>("DealerCommisionList", DealerCommisionList));
            return Ok(list);
        }
        [Route("GetSlipsAndDeliverySheetData")]
        public IHttpActionResult GetSlipsAndDeliverySheetData()
        {
            //-------------- Total Pending Slips
            var PendingSlipTotal = db.Society_Slip.Where(x => x.Letter_Status == "Pending").AsQueryable().Count();
            var BookingPendingSlipTotal = db.Society_Slip.Where(x => x.Letter_Status == "Pending" && x.Slip_Type == "Booking").AsQueryable().Count();
            var ConfirmationPendingSlipTotal = db.Society_Slip.Where(x => x.Letter_Status == "Pending" && x.Slip_Type == "Confirmation").AsQueryable().Count();
            var BookingSlipTotal = db.Society_Slip.Where(x => x.Slip_Type == "Booking").AsQueryable().Count();
            var ConfirmationSlipTotal = db.Society_Slip.Where(x => x.Slip_Type == "Confirmation").AsQueryable().Count();
            var BookingPercentageSlips = (decimal)((BookingPendingSlipTotal / BookingSlipTotal) * 100);
            var ConfirmationPercentageSlips = (decimal)((ConfirmationPendingSlipTotal / ConfirmationSlipTotal) * 100);
            //--------------End Total Pending Slips
           
            //-------------- Total Pending Slips
            var PendingDeliverySheetTotal = db.Delivery_Sheet.Where(x => x.Delivery_Status == "UnDelivered").AsQueryable().Count();
            var UnDeliveredBookingDeliverySheetTotal = db.Delivery_Sheet.Where(x => x.Delivery_Status == "UnDelivered" && x.Delivery_Type == "Booking").AsQueryable().Count();
            var ConfirmationUnDeleiveredDeliverySheetTotal = db.Delivery_Sheet.Where(x => x.Delivery_Status == "UnDelivered" && x.Delivery_Type == "Confirmation").AsQueryable().Count();
            var BookingDeliverySheetTotal = db.Delivery_Sheet.Where(x => x.Delivery_Type == "Booking").AsQueryable().Count();
            var ConfirmationDeliverySheetTotal = db.Delivery_Sheet.Where(x => x.Delivery_Type == "Confirmation").AsQueryable().Count();
            decimal BookingPercentageDeliverySheet = 0;
            decimal ConfirmationPercentageDeliverySheet = 0;
            if (UnDeliveredBookingDeliverySheetTotal == 0 || BookingDeliverySheetTotal == 0 || ConfirmationUnDeleiveredDeliverySheetTotal == 0 || ConfirmationDeliverySheetTotal == 0)
            {

            }
            else
            {
                BookingPercentageDeliverySheet = (decimal)((UnDeliveredBookingDeliverySheetTotal / BookingDeliverySheetTotal) * 100);
                ConfirmationPercentageDeliverySheet = (decimal)((ConfirmationUnDeleiveredDeliverySheetTotal / ConfirmationDeliverySheetTotal) * 100);
            }
            //--------------End Total Pending Slips

           
            var list = new List<Tuple<string, string>>();
            list.Add(new Tuple<string, string>("PendingSlipTotal", PendingSlipTotal.ToString()));
            list.Add(new Tuple<string, string>("BookingPendingSlipTotal", BookingPendingSlipTotal.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationPendingSlipTotal", ConfirmationPendingSlipTotal.ToString()));
            list.Add(new Tuple<string, string>("BookingSlipTotal", BookingSlipTotal.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationSlipTotal", ConfirmationSlipTotal.ToString()));
            list.Add(new Tuple<string, string>("BookingPercentageSlips", BookingPercentageSlips.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationPercentageSlips", ConfirmationPercentageSlips.ToString()));
            
            list.Add(new Tuple<string, string>("PendingDeliverySheetTotal", PendingDeliverySheetTotal.ToString()));
            list.Add(new Tuple<string, string>("UnDeliveredBookingDeliverySheetTotal", UnDeliveredBookingDeliverySheetTotal.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationUnDeleiveredDeliverySheetTotal", ConfirmationUnDeleiveredDeliverySheetTotal.ToString()));
            list.Add(new Tuple<string, string>("BookingDeliverySheetTotal", BookingDeliverySheetTotal.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationDeliverySheetTotal", ConfirmationDeliverySheetTotal.ToString()));
            list.Add(new Tuple<string, string>("BookingPercentageDeliverySheet", BookingPercentageDeliverySheet.ToString()));
            list.Add(new Tuple<string, string>("ConfirmationPercentageDeliverySheet", ConfirmationPercentageDeliverySheet.ToString()));
           return Ok(list);
        }
        [Route("GetSlipsAndDeliverySheetListData")]
        public IHttpActionResult GetSlipsAndDeliverySheetListData()
        {
            var propertListWithref_Value = db.PropertyDefs.Select(x => new PropertyAndRefList
            {
                Name = x.Name,
                Ref_Num = db.BookingConfirms.Where(z => z.Property_ID == x.ID).Select(z => z.Ref_num).AsQueryable().ToList()
            }).AsQueryable().ToList();
           // List<SocietySlipDTODashboard> societySlipDTODashboards = new List<SocietySlipDTODashboard>();
            var societySlipDTODashboards = propertListWithref_Value.Select(x => new SocietySlipDTODashboard
            {
             Name = x.Name,
            totalSlips = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num)).Select(s=>s.ID).AsQueryable().Count(),
            totalPendingSlips = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Letter_Status == "Pending").Select(s => s.ID).AsQueryable().Count(),
            totalDeliveredSlips = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Letter_Status == "Received").Select(s => s.ID).AsQueryable().Count(),
            totalBookingSlips = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Booking").Select(s => s.ID).AsQueryable().Count(),
            totalConfirmSlips = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Confirmation").Select(s => s.ID).AsQueryable().Count(),
            totalBookingSlipsAmount = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Booking").AsQueryable().Sum(s => s.Receipt_Amount),
            totalConfirmationSlipsAmount = db.Society_Slip.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Confirmation").AsQueryable().Sum(s => s.Receipt_Amount),
             }).AsQueryable().ToList();
            //foreach (var item in propertListWithref_Value)
            //{
            //    SocietySlipDTODashboard societySlipDTODashboard = new SocietySlipDTODashboard();
            //    societySlipDTODashboard.Name = item.Name;
            //    societySlipDTODashboard.totalSlips = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num)).Count();
            //    societySlipDTODashboard.totalPendingSlips = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Letter_Status == "Pending").Count();
            //    societySlipDTODashboard.totalDeliveredSlips = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Letter_Status == "Received").Count();
            //    societySlipDTODashboard.totalBookingSlips = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Booking").Count();
            //    societySlipDTODashboard.totalConfirmSlips = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Confirmation").Count();
            //    societySlipDTODashboard.totalBookingSlipsAmount = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Booking").Sum(s => s.Receipt_Amount);
            //    societySlipDTODashboard.totalConfirmationSlipsAmount = db.Society_Slip.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Slip_Type == "Confirmation").Sum(s => s.Receipt_Amount);
            //    societySlipDTODashboards.Add(societySlipDTODashboard);
            //}
            //List<SocietySlipDTODashboard> DleiveySheetsDTODashboards = new List<SocietySlipDTODashboard>();
            var DleiveySheetsDTODashboards = propertListWithref_Value.Select(x => new SocietySlipDTODashboard
            {
                Name = x.Name,
                totalSlips = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num)).AsQueryable().Count(),
                totalPendingSlips = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Status == "UnDelivered").Select(s => s.ID).AsQueryable().Count(),
                totalDeliveredSlips = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Status == "Delivered").Select(s => s.ID).AsQueryable().Count(),
                totalBookingSlips = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Booking").Select(s => s.ID).AsQueryable().Count(),
                totalConfirmSlips = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Confirmation").Select(s => s.ID).AsQueryable().Count(),
                totalBookingSlipsAmount = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Booking").AsQueryable().Sum(s => s.Total_Amount),
                totalConfirmationSlipsAmount = db.Delivery_Sheet.Where(s => x.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Confirmation").AsQueryable().Sum(s => s.Total_Amount),
            }).AsQueryable().ToList();
            //foreach (var item in propertListWithref_Value)
            //{
            //    SocietySlipDTODashboard DleiveySheetsDTODashboard = new SocietySlipDTODashboard();
            //    DleiveySheetsDTODashboard.Name = item.Name;
            //    DleiveySheetsDTODashboard.totalSlips = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num)).Count();
            //    DleiveySheetsDTODashboard.totalPendingSlips = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Status == "UnDelivered").Count();
            //    DleiveySheetsDTODashboard.totalDeliveredSlips = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Status == "Delivered").Count();
            //    DleiveySheetsDTODashboard.totalBookingSlips = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Booking").Count();
            //    DleiveySheetsDTODashboard.totalConfirmSlips = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Confirmation").Count();
            //    DleiveySheetsDTODashboard.totalBookingSlipsAmount = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Booking").Sum(s => s.Total_Amount);
            //    DleiveySheetsDTODashboard.totalConfirmationSlipsAmount = db.Delivery_Sheet.Where(s => item.Ref_Num.Contains(s.Ref_num) && s.Delivery_Type == "Confirmation").Sum(s => s.Total_Amount);
            //    DleiveySheetsDTODashboards.Add(DleiveySheetsDTODashboard);
            //}

            var list = new List<Tuple<string, List<SocietySlipDTODashboard>>>();
            list.Add(new Tuple<string, List<SocietySlipDTODashboard>>("SocietySlips", societySlipDTODashboards));
            list.Add(new Tuple<string, List<SocietySlipDTODashboard>>("DleiveySheets", DleiveySheetsDTODashboards));
            return Ok(list);
        }
    }
}

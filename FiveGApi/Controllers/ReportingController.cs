using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Reporting")]
    public class ReportingController : ApiController
    {
        [Route("MasterSheetReport")]
        public HttpResponseMessage MasterSheetReport(DateTime? StartDate, DateTime? EndDate, decimal? PlotStartingPrice, decimal? PlotEndingPrice
            , string Category, DateTime? LetterRecivingStartDate, DateTime? LetterRecivingEndDate,
            string LetterStatus, decimal? BookingStartingPrice, decimal? BookingEndingPrice, string BookingStatus,
            decimal? ConfirmationStartingPrice, decimal? ConfirmationEndingPrice, string ConfirmationStatus)
        {
            try
            {
                if(StartDate==null)
                {
                    StartDate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                }
                if (EndDate == null)
                {
                    EndDate = Convert.ToDateTime("2100-01-01 00:00:00.000");
                }
                if (LetterRecivingStartDate == null)
                {
                    LetterRecivingStartDate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                }
                if (LetterRecivingEndDate == null)
                {
                    LetterRecivingEndDate = Convert.ToDateTime("2100-01-01 00:00:00.000");
                }
                if (Category == "null")
                {
                    Category = null;
                }
                if (BookingStatus == "null")
                {
                    BookingStatus = null;
                }
                if (ConfirmationStatus == "null")
                {
                    ConfirmationStatus = null;
                }
                if (LetterStatus == "null")
                {
                    LetterStatus = null;
                }
                
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/MasterSheetReport2.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = reportDateWithParameters(StartDate, EndDate,  PlotStartingPrice,  PlotEndingPrice
            ,  Category,  LetterRecivingStartDate,  LetterRecivingEndDate,
             LetterStatus,  BookingStartingPrice,  BookingEndingPrice,  BookingStatus,
             ConfirmationStartingPrice,  ConfirmationEndingPrice, ConfirmationStatus);
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "MasterSheetReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch( Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        [Route("MasterSheetProjectReport")]
        public HttpResponseMessage MasterSheetProjectReport(string ProjectType, int? Units, string PaymentMileStone, int? DealerID
           , int? EmployeeID)
        {
            try
            {
                
                if (ProjectType == "null")
                {
                    ProjectType = null;
                }
                if (Units == 0)
                {
                    Units = null;
                }
                if (PaymentMileStone == "null")
                {
                    PaymentMileStone = null;
                }
                if (EmployeeID == 0)
                {
                    EmployeeID = null;
                }
                if (DealerID == 0)
                {
                    DealerID = null;
                }

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/MasterProjectReportData.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = ProjectReportDateWithParameters( ProjectType, Units,  PaymentMileStone,  DealerID , EmployeeID);
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "ProjectMasterSheetReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch (Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        [Route("TrailBalanceReport")]
        public HttpResponseMessage TrailBalanceReport()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/TrailBalanceReport.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = reportDate("TrailBalancesData");
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "TrailBalanceReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch (Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        [Route("TrailBalanceSecondReport")]
        public HttpResponseMessage TrailBalanceSecondReport(DateTime? startDate, DateTime? endDate, string segment, int parentAccount)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/Trail.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = reportDateWithParameters("TrailBalancesDataByDate",startDate, endDate,segment,parentAccount);
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "TrailBalanceWithDateStampsReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch (Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        [Route("BalanceSheetReport")]
        public HttpResponseMessage BalanceSheetReport()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/BalanceSheetReport.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = reportDate("BalanceSheetData");
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "BalanceSheetReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch (Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        [Route("ProfiLossReport")]
        public HttpResponseMessage ProfiLossReport()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                ReportViewer ReportViewerRSFReports = new ReportViewer();
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";
                var rptPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Reports/PandLReport.rdlc");
                ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                DataTable dt = reportDate("ProfitLossSheetData");
                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                ////Revenue Parameter
                //ReportParameter Revenue = new ReportParameter();
                //Revenue.Name = "@Revenue";
                //double rValue = 911.0;//new Uri(Server.MapPath(@"~/" + logoPath.FirstOrDefault().Logo)).AbsoluteUri;
                //Revenue.Values.Add(rValue.ToString());
                //ReportViewerRSFReports.LocalReport.SetParameters(Revenue);
                ////Expense
                //ReportParameter Expense = new ReportParameter();
                //Expense.Name = "@Expense";
                //double eValue = 909.0;//new Uri(Server.MapPath(@"~/" + logoPath.FirstOrDefault().Logo)).AbsoluteUri;
                //Revenue.Values.Add(eValue.ToString());
                //ReportViewerRSFReports.LocalReport.SetParameters(Expense);
                //double rValue = 911.0;
                //ReportParameter rp = new ReportParameter("Revenue", rValue.ToString()); 
                //double eValue = 911.0;
                //ReportParameter ep = new ReportParameter("Expense", eValue.ToString());
                ReportViewerRSFReports.LocalReport.DataSources.Clear();
                //ReportViewerRSFReports.LocalReport.SetParameters(new ReportParameter[] { rp, ep });
                ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                //ReportViewerRSFReports.LocalReport.SetParameters(new ReportParameter[] { rp,ep });
                ReportViewerRSFReports.LocalReport.Refresh();
                byte[] mybytes = ReportViewerRSFReports.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                FileStream file;//=new FileStream();
                string path2 = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Attachments\NewFiles\");
                string fileName = "ProfitLossReport-" + DateTime.Now.ToFileTime() + ".xls";
                dynamic savePath2 = (path2 + fileName);
                file = new FileStream(savePath2, FileMode.Create);
                file.Write(mybytes, 0, mybytes.Length);
                file.Close();
                file.Dispose();
                byte[] bytes = File.ReadAllBytes(savePath2);
                HttpResponseMessage response = null;
                //Create HTTP Response.
                response = Request.CreateResponse(HttpStatusCode.OK);
                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                return response;
            }
            catch (Exception ex)
            {
                var message = String.Format("Not Found Or Error Message", ex.Message);
                var errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(errorResponse);
            }
        }
        private DataTable reportDate(string procedureName)
        {
            DataTable dt = new DataTable();
            var conString = ConfigurationManager.ConnectionStrings["mis_dbConnectionString"].ToString(); ;
            SqlCommand cmd = new SqlCommand(procedureName);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand = cmd;
                    using (DataSet dsCustomers = new DataSet())
                    {
                        sda.Fill(dsCustomers, "Customers");
                        return dsCustomers.Tables[0];
                    }
                }
            }
        }
        private DataTable reportDateWithParameters(string procedureName,DateTime? startDate, DateTime? endDate,string segment,int parentAccount )
        {
            DataTable dt = new DataTable();
            var conString = ConfigurationManager.ConnectionStrings["mis_dbConnectionString"].ToString(); ;
            SqlCommand cmd = new SqlCommand(procedureName);
            cmd.Parameters.AddWithValue("@StartDate", startDate); //"2021-03-27 08:54:04.323"
            cmd.Parameters.AddWithValue("@EndDate", endDate);//"2021-04-30 06:52:18.193"
            cmd.Parameters.AddWithValue("@Segment", segment); //"Account"
            cmd.Parameters.AddWithValue("@ParentAccount", parentAccount); //27

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand = cmd;
                    using (DataSet dsCustomers = new DataSet())
                    {
                        sda.Fill(dsCustomers, "Customers");
                        return dsCustomers.Tables[0];
                    }
                }
            }
        }
        
        private DataTable reportDateWithParameters(DateTime? StartDate,DateTime? EndDate,decimal? PlotStartingPrice,decimal? PlotEndingPrice
            ,string Category,DateTime? LetterRecivingStartDate,DateTime? LetterRecivingEndDate,
            string LetterStatus, decimal? BookingStartingPrice,decimal? BookingEndingPrice, string BookingStatus,
            decimal? ConfirmationStartingPrice,decimal? ConfirmationEndingPrice,string ConfirmationStatus)
        {
            DataTable dt = new DataTable();
            var conString = ConfigurationManager.ConnectionStrings["mis_dbConnectionString"].ToString(); ;
            SqlCommand cmd = new SqlCommand("MasterSheetData");
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@PlotStartingPrice", PlotStartingPrice);
            cmd.Parameters.AddWithValue("@PlotEndingPrice", PlotEndingPrice);
            cmd.Parameters.AddWithValue("@Category", Category);
            cmd.Parameters.AddWithValue("@LetterRecivingStartDate", LetterRecivingStartDate);
            cmd.Parameters.AddWithValue("@LetterRecivingEndDate", LetterRecivingEndDate);
            cmd.Parameters.AddWithValue("@LetterStatus", LetterStatus);
            cmd.Parameters.AddWithValue("@BookingStartingPrice", BookingStartingPrice);
            cmd.Parameters.AddWithValue("@BookingEndingPrice", BookingEndingPrice);
            cmd.Parameters.AddWithValue("@BookingStatus", BookingStatus);
            cmd.Parameters.AddWithValue("@ConfirmationStartingPrice", ConfirmationStartingPrice);
            cmd.Parameters.AddWithValue("@ConfirmationEndingPrice", ConfirmationEndingPrice);
            cmd.Parameters.AddWithValue("@ConfirmationStatus", ConfirmationStatus);

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand = cmd;
                    using (DataSet dsCustomers = new DataSet())
                    {
                        sda.Fill(dsCustomers, "Customers");
                        return dsCustomers.Tables[0];
                    }
                }
            }
        }
        private DataTable ProjectReportDateWithParameters(string ProjectType, int? Unit, string PaymentMileStone, int? DealerID
           , int? EmployeeID) {
            DataTable dt = new DataTable();
            var conString = ConfigurationManager.ConnectionStrings["mis_dbConnectionString"].ToString(); ;
            SqlCommand cmd = new SqlCommand("MasterSheetProjectData");
            cmd.Parameters.AddWithValue("@ProjectType", ProjectType);
            cmd.Parameters.AddWithValue("@Unit", Unit);
            cmd.Parameters.AddWithValue("@PaymentMileStone", PaymentMileStone);
            cmd.Parameters.AddWithValue("@DealerID", DealerID);
            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand = cmd;
                    using (DataSet dsCustomers = new DataSet())
                    {
                        sda.Fill(dsCustomers, "Customers");
                        return dsCustomers.Tables[0];
                    }
                }
            }
        }
    }
}

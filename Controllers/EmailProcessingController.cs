using NOCEntities;
using NOCEntities.ISD;
using NOCEntitiesBL;
using NOCEntitiesBL.ISD;
using NOCMvc.Models;
using NOCMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NOCEntitiesBL.TeamUserMapping;
using NOCEntities.TeamUserMapping;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web.Caching;

namespace NOCMvc.Controllers
{
    public class EmailProcessingController : Controller//BaseController<VMUnprocessedEmail, UnprocessedEmail>
    {
        private const string connectionString = "EmailToTktConnection";
        private const string connectionStringDefault = "DefaultConnection";
        UnprocessedEmailBL unprocessedEmailBL = new UnprocessedEmailBL(connectionString);
        ConnectionStringsSection connectionStringSection = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
        public ITeamService TeamService = null;
        public IEnumerable<Team> oNocTeams { get; set; }
        public Team oNocTeam { get; set; }
        VMUnprocessedEmail vmUnprocessedEmail;
        private List<UnprocessedEmail> ListEmails { get; set; }

        public EmailProcessingController()
        {
            this.TeamService = new TeamService(new TeamDal());
        }
        //
        // GET: /UnparsedEmail/
        public ActionResult Index()
        {
            try
            {
                //var EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);

                //ViewData["MailCount"] = EmailList.Count;
                vmUnprocessedEmail = new VMUnprocessedEmail();
                Create();
                return View("List", vmUnprocessedEmail);
            }
            catch (Exception ex)
            {
                ViewData["MailCount"] = 0;
                return View();
            }
        }

        //
        // GET: /UnparsedEmail/Details/5
        public ActionResult List([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                Session.Clear();

                var EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);
                //vmUnprocessedEmail.EmailList = new List<UnprocessedEmail>();
                //vmUnprocessedEmail.EmailList.Add(new UnprocessedEmail() { ID = 1, FromEmail = "sss@ss.com", EmailSubject = "abc", EmailBody = "aaa", Date = DateTime.Now.ToString() });
                //vmUnprocessedEmail.EmailList.Add(new UnprocessedEmail() { ID = 2, FromEmail = "ssss@ssa.com", EmailSubject = "abcd", EmailBody = "adsd", Date = DateTime.Now.ToString() });

                Session.Add("EmailList", EmailList);
                Session.Add("SelectedDataId", EmailList[0].ID.ToString());
                //Cache cache = new Cache();
                //cache.Add("EmailList", EmailList, new CacheDependency(@"c:\test.txt"), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(2), CacheItemPriority.High,new CacheItemRemovedCallback(OnRemoveCache));
                
                DataSourceResult result = EmailList.ToDataSourceResult(request);
                JsonResult json = Json(result, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json; //View(vmUnprocessedEmail);

            }
            catch (Exception ex)
            {
                ViewData["MailCount"] = 0;
                return View("Index");
            }
        }

        //void OnRemoveCache(string test,object sender, CacheItemRemovedReason reason)
        //{
        //    Response.Write("");
        //}

        /*[HttpGet]
        public JsonResult GetString(int Id)
        {
            return Json("Test" + Id, JsonRequestBehavior.AllowGet);
        }*/

        [HttpPost]
        public JsonResult SetItemId(int Id)
        {
            Session.Add("SelectedDataId", Id.ToString());
            return Json("Added ID", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMspList(string mspName)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public JsonResult GetTicketIdList(int ticketId)
        {
            List<string> listTaskId = new List<string>();
            SqlCommand cmd = new SqlCommand("Select top 10 TaskID from jmgtTaskManagement where TaskID like '" + ticketId + "%'", new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
            cmd.Connection.Open();
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listTaskId.Add(reader["TaskID"].ToString());
            }
            return Json(listTaskId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmailBody(string Id)
        {
            // var EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);
            
            //Cache cache = new Cache();
            List<UnprocessedEmail> list = (List<UnprocessedEmail>)Session["EmailList"];
            string emailBody = list.Find(email => email.ID == Id.ToString()).EmailBody;
           
            return Json(emailBody, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /UnparsedEmail/Create
        [HttpGet]
        public ActionResult Create()
        {
            //oNocTeams = this.TeamService.GetTeamList();

            //if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            //{
            //    return RedirectToAction("List");
            //}
            //string Id = Session["SelectedDataId"].ToString();
            TicketForUnparsedMail ticket = new TicketForUnparsedMail();
            //List<UnprocessedEmail> emailList = (List<UnprocessedEmail>)Session["EmailList"];

            SqlCommand cmd = new SqlCommand("Get_Status_Category_TaskType", new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
            cmd.Connection.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Flag", 4));
            var reader = cmd.ExecuteReader();

            ticket.listCategory = new Dictionary<string, string>();

            while (reader.Read())
            {
                ticket.listCategory.Add(reader["CategoryName"].ToString(), reader["CategoryName"].ToString());
            }
            reader.Close();
            cmd.Dispose();
            Session.Add("categories", ticket.listCategory);
            var unprocessedEmail = new NOCEntities.UnprocessedEmail();//emailList.Find(e => e.ID == Id);
            //ticket.UnParsedEmailBody = unprocessedEmail.EmailBody;
            //ticket.UnParsedEmailSubject = unprocessedEmail.EmailSubject;
            List<SelectListItem> lstItems = new List<SelectListItem>() { new SelectListItem { Text = "<--Select-->", Value = "<--Select-->" } };
            ViewData["emptyList"] = lstItems;

            vmUnprocessedEmail.ticketForEmail = ticket;

            return View(ticket);

            //MSP search - frmMSPFilter.asp
        }

        //
        // POST: /UnparsedEmail/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            {
                return RedirectToAction("List");
            }
            try
            {
                TicketServiceReference.Ticket ticketAPI = new TicketServiceReference.Ticket();

                long TicketId = GenerateTicketData(collection);

                if (TicketId > 0)
                {
                    SqlCommand cmd = new SqlCommand("USP_Update_Unprocessed_Emails_Error_Flag", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UID", Session["SelectedDataId"].ToString()));
                    cmd.Parameters.Add(new SqlParameter("@Error", ""));
                    cmd.Parameters.Add(new SqlParameter("@TicketFlag", 1));
                    cmd.Parameters.Add(new SqlParameter("@TaskId", TicketId));
                    int records = cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    cmd = new SqlCommand("USP_Move_EmailTkt_To_History", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    Session.Clear();
                    VMUnprocessedEmail vmUnprocessedEmail = new VMUnprocessedEmail();
                    vmUnprocessedEmail.EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);
                    Session.Add("EmailList", vmUnprocessedEmail.EmailList);
                    ViewBag.TicketId = TicketId;
                    return View("List", vmUnprocessedEmail);
                }
                else
                {
                    return DefaultList("Some error on API side while creating ticket");
                }
            }
            catch (Exception ex)
            {
                return DefaultList(ex.Message.ToString());
            }
        }

        private ActionResult DefaultList(string message, bool IsException = true)
        {
            Session.Clear();
            VMUnprocessedEmail vmUnprocessedEmail = new VMUnprocessedEmail();
            vmUnprocessedEmail.EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);
            Session.Add("EmailList", vmUnprocessedEmail.EmailList);
            if (IsException)
                ViewData["exception"] = message;
            else
                ViewData["Discarded"] = message;
            return View("List", vmUnprocessedEmail);
        }

        //[HttpPost]
        //public JsonResult SearchMSP(string searchStr)
        //{
        //    string memberName, memberCode, memberId = "";

        //    SqlCommand cmd = new SqlCommand("USP_GetPartnerList_GlobalPSA_PR", new SqlConnection("Server=MUMPSQL01; Database=NOCBO; uid=its;pwd=its;Connect Timeout=120;"));
        //    cmd.Connection.Open();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@strmember", searchStr));
        //    var reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        memberName = reader["MemberName"].ToString();
        //        memberCode = reader["MemberCode"].ToString();
        //        memberId = reader["Memberid"].ToString();



        //        break;
        //    }

        //    SqlCommand cmd1 = new SqlCommand("Select SiteName,SiteCode from MSTSite where Memberid = " + memberId, new SqlConnection("Server=MUMPSQL01; Database=NOCBO; uid=its;pwd=its;Connect Timeout=120;"));
        //    cmd1.Connection.Open();
        //    cmd1.CommandType = CommandType.Text;
        //    var reader1 = cmd1.ExecuteReader();

        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    while (reader1.Read())
        //    {
        //        lst.Add(new SelectListItem { Text = reader1["SiteName"].ToString(), Value = reader1["SiteCode"].ToString() });
        //    }

        //    return Json("MSP1", JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult SearchMSP(FormCollection collection)
        {
            try
            {
                string memberName = "", memberCode, memberId = "";

                SqlCommand cmd = new SqlCommand("USP_GetPartnerList_GlobalPSA_PR", new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@strmember", collection["txtSrch"]));
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    memberName = reader["MemberName"].ToString();
                    memberCode = reader["MemberCode"].ToString();
                    memberId = reader["Memberid"].ToString();
                    break;
                }
                reader.Close();
                cmd.Dispose();
                Dictionary<string, string> lst = null;
                if (String.IsNullOrEmpty(memberId) == false)
                {
                    SqlCommand cmd1 = new SqlCommand("Select SiteName,SiteCode from MSTSite where Memberid = " + memberId, new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
                    cmd1.Connection.Open();
                    cmd1.CommandType = System.Data.CommandType.Text;
                    var reader1 = cmd1.ExecuteReader();

                    lst = new System.Collections.Generic.Dictionary<string, string>();
                    while (reader1.Read())
                    {
                        if (lst.ContainsKey(reader1["SiteName"].ToString()) == false)
                            lst.Add(reader1["SiteName"].ToString(), reader1["SiteCode"].ToString());
                    }
                    reader1.Close();
                    cmd1.Dispose();

                }
                if (Session["SelectedDataId"] != null && Session["EmailList"] != null)
                {
                    string Id = Session["SelectedDataId"].ToString();
                    TicketForUnparsedMail ticket = new TicketForUnparsedMail();
                    List<UnprocessedEmail> emailList = (List<UnprocessedEmail>)Session["EmailList"];
                    var unprocessedEmail = emailList.Find(e => e.ID == Id);
                    ticket.UnParsedEmailBody = unprocessedEmail.EmailBody;
                    ticket.UnParsedEmailSubject = unprocessedEmail.EmailSubject;
                    ticket.listCategory = (Dictionary<string, string>)Session["categories"];
                    ticket.MSPName = memberName;
                    ticket.Resource = "Resource1";
                    ticket.listSites = lst;
                    return View("Create", ticket);
                }
                else
                    return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return DefaultList(ex.Message.ToString());
            }
        }

        [HttpPost]
        public ActionResult SearchTicket(FormCollection collection)
        {
            if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            {
                return RedirectToAction("List");
            }
            try
            {
                string memberName = "", memberCode, memberId = "";
                TicketForUnparsedMail ticket = new TicketForUnparsedMail();
                var ticketId = collection["tktidtext"];

                SqlCommand cmd = new SqlCommand("IDWiseNOCManagementRepsp_NOC", new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@strid", ticketId));
                cmd.Parameters.Add(new SqlParameter("@TicketTask", 2));
                cmd.Parameters.Add(new SqlParameter("@InFlag", 1));

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ticket.MSPName = reader["GroupName"].ToString();
                    ticket.MSPSite = reader["Client"].ToString();
                    ticket.TicketSubject = reader["taskSubject"].ToString();
                    ticket.TicketDescription = reader["taskDescription"].ToString();
                    break;
                }

                reader.Close();
                cmd.Dispose();
                reader = null;

                cmd = new SqlCommand("Select top 1 TaskComments from jmgtTaskNotes where TaskID = '" + ticketId + "' ORDER BY TransDate DESC", new SqlConnection(connectionStringSection.ConnectionStrings[connectionStringDefault].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.Text;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ticket.TicketNotes = reader["TaskComments"].ToString();
                }

                string Id = Session["SelectedDataId"].ToString();

                List<UnprocessedEmail> emailList = (List<UnprocessedEmail>)Session["EmailList"];
                var unprocessedEmail = emailList.Find(e => e.ID == Id);
                ticket.UnParsedEmailBody = unprocessedEmail.EmailBody;
                ticket.TicketId = ticketId;
                return View("update", ticket);
            }
            catch (Exception ex)
            {
                return DefaultList(ex.Message.ToString());
            }
        }

        //
        // GET: /UnparsedEmail/Update/5
        public ActionResult Update()
        {
            if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            {
                return RedirectToAction("List");
            }

            string Id = Session["SelectedDataId"].ToString();
            TicketForUnparsedMail ticket = new TicketForUnparsedMail();
            List<UnprocessedEmail> emailList = (List<UnprocessedEmail>)Session["EmailList"];

            var unprocessedEmail = emailList.Find(e => e.ID == Id);
            ticket.UnParsedEmailBody = unprocessedEmail.EmailBody;
            ticket.UnParsedEmailSubject = unprocessedEmail.EmailSubject;

            return View("Update", ticket);
        }

        //
        // POST: /UnparsedEmail/Update/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(FormCollection collection)
        {
            if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            {
                return RedirectToAction("List");
            }
            try
            {
                // TODO: Add update logic here
                long TicketId = UpdateTicketData(collection);

                SqlCommand cmd = new SqlCommand("USP_Update_Unprocessed_Emails_Error_Flag", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UID", Session["SelectedDataId"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@Error", ""));
                cmd.Parameters.Add(new SqlParameter("@TicketFlag", 2));
                cmd.Parameters.Add(new SqlParameter("@TaskId", TicketId));

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = new SqlCommand("USP_Move_EmailTkt_To_History", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Session.Clear();

                VMUnprocessedEmail vmUnprocessedEmail = new VMUnprocessedEmail();
                vmUnprocessedEmail.EmailList = unprocessedEmailBL.GetUnprocessedEmailList(connectionString);
                Session.Add("EmailList", vmUnprocessedEmail.EmailList);
                ViewBag.TicketId = TicketId;
                ViewBag.IsUpdated = "True";
                return View("List", vmUnprocessedEmail);

            }
            catch (Exception ex)
            {
                return DefaultList(ex.Message);
            }
        }

        //
        // GET: /UnparsedEmail/Discard/5

        public ActionResult Discard()
        {
            if (Session["SelectedDataId"] != null)
                return View("Discard");
            else
                return RedirectToAction("List");
        }

        //
        // POST: /UnparsedEmail/Discard/5
        [HttpPost]
        public ActionResult Discard(FormCollection collection)
        {
            if (Session["SelectedDataId"] == null || Session["EmailList"] == null)
            {
                return RedirectToAction("List");
            }
            try
            {
                // TODO: Add delete logic here
                SqlCommand cmd = new SqlCommand("USP_Update_Unprocessed_Emails_Error_Flag", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UID", Session["SelectedDataId"].ToString()));
                cmd.Parameters.Add(new SqlParameter("@Error", collection["txtDiscardReason"]));
                cmd.Parameters.Add(new SqlParameter("@TicketFlag", 3));
                int records = cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = new SqlCommand("USP_Move_EmailTkt_To_History", new SqlConnection(connectionStringSection.ConnectionStrings[connectionString].ConnectionString));
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Session.Clear();

                return DefaultList("Email discarded with reason: " + collection["txtDiscardReason"], false);
            }
            catch (Exception ex)
            {
                return DefaultList(ex.Message.ToString());
            }
        }

        private long GenerateTicketData(FormCollection collection)
        {


            //TicketServiceReference.Ticket ticketAPI = new TicketServiceReference.Ticket();

            //TicketServiceReference.TicketList ticketList = new TicketServiceReference.TicketList();

            /*This is for DT reference*/



            DTTicketServiceReference.Ticket ticketAPI = new DTTicketServiceReference.Ticket();

            DTTicketServiceReference.TicketList ticketList = new DTTicketServiceReference.TicketList();
            /*End This is for DT reference*/

            DateTime CTime = DateTime.Now;

            CreateTicketParam ticket = new CreateTicketParam();


            int i = 0;

            SiteTimeZoneInfo ClientSiteTimeZoneInfo = GetUTCTimediff(connectionString, "");

            ticketAPI.MemberName = "CMSPLDEV";
            //ticketAPI.MemberName = collection["MSPName"];
            //ticketAPI.MemberCode = "CMSPLDEV";

            //ticketAPI.Site = new TicketServiceReference.Site() { Code = "continuum", SubLocation = "" };//ticket.Site;

            /*This is for DT reference*/
            ticketAPI.Site = new DTTicketServiceReference.Site() { Code = "Delhi-site", SubLocation = "" };//ticket.Site;
            //ticketAPI.Site = new DTTicketServiceReference.Site() { Code = collection["MSPSite"], SubLocation = "" };//ticket.Site;

            ticketAPI.ResourceName = "Resource1";
            ticketAPI.CreateTicketType = "ServiceRequest";
            //MSTAlertFamily - id and name - check NOC portal SP 
            ticketAPI.CategoryId = 2;// Convert.ToInt32(dr["CategoryId"]); //- from family name dropdown
            ticketAPI.CategoryName = collection["FamilyName"];//- from family name dropdown
            ticketAPI.CategoryIdSpecified = true;

            ticketAPI.Subject = collection["UnParsedEmailSubject"];
            ticketAPI.Description = collection["UnParsedEmailBody"];
            ticketAPI.InternalNocConv = "";

            ticketAPI.Priority = int.Parse(collection["TicketPriority"]);//Convert.ToInt32(dr["Priority"]);//set from UI
            ticketAPI.StatusId = 101;
            ticketAPI.StatusName = "new";
            ticketAPI.PrioritySpecified = true;
            ticketAPI.StatusIdSpecified = true;
            ticketAPI.AssignTo = "NOC Team";
            ticketAPI.AssignToGroupId = 23; //select top 10 * from Groupmaster with(nolock) where GroupName = 'SRT'
            ticketAPI.AssignToGroupIdSpecified = true;
            //ticketAPI.AssignToGroupId = Convert.ToInt32(dr["AssignToGroup"]);
            //ticketAPI.AssignToGroupIdSpecified = true;
            ticketAPI.ExecutionDate = DateTime.Now;
            ticketAPI.ExecutionDateSpecified = true;
            ticketAPI.Date = DateTime.Now;
            ticketAPI.DateSpecified = true;
            ticketAPI.OriginOfTheCall = "Email";
            //ticketAPI.AssignToUser = dr["AssignToUser"].ToString() == "" ? 0 : Convert.ToInt32(dr["AssignToUser"].ToString());
            //ticketAPI.AssignToUserSpecified = true;
            ticketAPI.GeneratedBy = "NOC Team";
            ticketAPI.CreatedBy = -5;
            //ticketAPI.AssignFromId = 22;
            //ticketAPI.AssignFromIdSpecified = true;
            ticketAPI.CreatedBySpecified = true;
            //ticketAPI.SubStatus = "";
            //ticketAPI.SubStatusSpecified = true;
            ticketAPI.TimeZone = "";// dr["TimeZone"].ToString();
            ticketAPI.UpdateFrequency = null;

            ticketAPI.OutOfScopeReasonID = 0;
            ticketAPI.OutOfScopeReasonIDSpecified = true;

            ticketAPI.Prerequisites = null;

            ticketAPI.SDEnabled = 0;
            ticketAPI.SDEnabledSpecified = true;

            //if (dr["ScheduledStartDate"] != null)
            //{

            //    ticketAPI.ScheduledStartDate = this.priorityService.GetPSTTimeAccordingToSiteTimeZone(ClientSiteTimeZoneInfo, dr["ScheduledStartDate"].ToString());
            //    ticketAPI.ScheduledStartDateSpecified = true;
            //    ticketAPI.PstTimeSchedule = Convert.ToDateTime(dr["ScheduledStartDate"]);
            //    ticketAPI.PstTimeScheduleSpecified = true;

            //}
            //if (dr["ScheduledEndDate"] != null)
            //{

            //    ticketAPI.ScheduledEndDate = this.priorityService.GetPSTTimeAccordingToSiteTimeZone(ClientSiteTimeZoneInfo, dr["ScheduledEndDate"].ToString());
            //    ticketAPI.ScheduledEndDateSpecified = true;
            //    ticketAPI.PstEndDate = Convert.ToDateTime(dr["ScheduledEndDate"]);
            //    ticketAPI.PstEndDateSpecified = true;
            //}


            // CreateTicketUsingApi(ticketAPI);

            return CreateDTTicketUsingApi(ticketAPI);/*This is for DT reference*/
        }

        private long CreateDTTicketUsingApi(DTTicketServiceReference.Ticket ticket1)
        {
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();


            sw2.Start();

            using (DTTicketServiceReference.AlertManagementService client1 = new DTTicketServiceReference.AlertManagementService())
            {
                ticket1 = client1.CreateTicket(ticket1);
            }
            sw2.Stop();
            //if (ticket1.Id != 0)
            //{
            //    results.Add(new Models.Priortization_generateticketViewModel { RequestNumber = i.ToString(), TicketNumber = ticket1.Id.ToString(), NumberOfTicketsCreated = 1, ElapsedMilliseconds = sw2.ElapsedMilliseconds });
            //}
            //else
            //{
            //    results.Add(new Models.Priortization_generateticketViewModel { RequestNumber = i.ToString(), TicketNumber = "No Ticket Generated", NumberOfTicketsCreated = 0, ElapsedMilliseconds = sw2.ElapsedMilliseconds });
            //}
            return ticket1.Id;

        }

        private void CreateTicketUsingApi(TicketServiceReference.Ticket ticket1)
        {
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();

            try
            {
                sw2.Start();

                using (TicketServiceReference.AlertManagementServiceClient client1 = new TicketServiceReference.AlertManagementServiceClient("WSHttpBinding_IAlertManagementService"))
                {
                    ticket1 = client1.CreateTicket(ticket1);
                }
                sw2.Stop();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long UpdateTicketData(FormCollection collection)
        {


            //TicketServiceReference.Ticket ticketAPI = new TicketServiceReference.Ticket();

            //TicketServiceReference.TicketList ticketList = new TicketServiceReference.TicketList();

            /*This is for DT reference*/



            //DTTicketServiceReference.Ticket ticketAPI = new DTTicketServiceReference.Ticket();

            //DTTicketServiceReference.TicketList ticketList = new DTTicketServiceReference.TicketList();
            ///*End This is for DT reference*/

            //DateTime CTime = DateTime.Now;

            //CreateTicketParam ticket = new CreateTicketParam();


            //int i = 0;

            //SiteTimeZoneInfo ClientSiteTimeZoneInfo = GetUTCTimediff(connectionString, "");

            //ticketAPI.MemberName = "CMSPLDEV";
            ////ticketAPI.MemberName = collection["MSPName"];
            ////ticketAPI.MemberCode = "CMSPLDEV";

            ////ticketAPI.Site = new TicketServiceReference.Site() { Code = "continuum", SubLocation = "" };//ticket.Site;

            ///*This is for DT reference*/
            //ticketAPI.Site = new DTTicketServiceReference.Site() { Code = "Delhi-site", SubLocation = "" };//ticket.Site;
            ////ticketAPI.Site = new DTTicketServiceReference.Site() { Code = collection["MSPSite"], SubLocation = "" };//ticket.Site;
            //ticketAPI.Id = long.Parse(collection["TicketId"]);
            //ticketAPI.IdSpecified = true;
            //ticketAPI.AssignTo = "NOC Team";
            //ticketAPI.Description = collection["UnParsedEmailBody"];

            DTTicketServiceReference.UpdateRawTicketOperationRequest request = new DTTicketServiceReference.UpdateRawTicketOperationRequest();
            request.Comments = collection["UnParsedEmailBody"];
            request.TaskRawId = collection["TicketId"];
            request.Assignedto = 1;
            request.PartnerCommunication = true;
            request.InternalCommunication = false;
            request.TicketStatusId = 0;
            request.ActionType = "";
            request.AssignedtoSpecified = true;

            UpdateDTTicketUsingApiForNOC(request);/*This is for DT reference*/
            return long.Parse(collection["TicketId"]);
        }

        private long UpdateDTTicketUsingApi(DTTicketServiceReference.Ticket ticket1)
        {
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();

            try
            {
                sw2.Start();

                using (DTTicketServiceReference.AlertManagementService client1 = new DTTicketServiceReference.AlertManagementService())
                {
                    ticket1 = client1.UpdateTicket(ticket1);
                    if (ticket1.Error != null)
                        Response.Write(ticket1.Error.message);

                }
                sw2.Stop();
                //if (ticket1.Id != 0)
                //{
                //    results.Add(new Models.Priortization_generateticketViewModel { RequestNumber = i.ToString(), TicketNumber = ticket1.Id.ToString(), NumberOfTicketsCreated = 1, ElapsedMilliseconds = sw2.ElapsedMilliseconds });
                //}
                //else
                //{
                //    results.Add(new Models.Priortization_generateticketViewModel { RequestNumber = i.ToString(), TicketNumber = "No Ticket Generated", NumberOfTicketsCreated = 0, ElapsedMilliseconds = sw2.ElapsedMilliseconds });
                //}
                return ticket1.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateDTTicketUsingApiForNOC(DTTicketServiceReference.UpdateRawTicketOperationRequest request)
        {
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            DTTicketServiceReference.UpdateRawTicketOperationResponse response = null;
            try
            {
                sw2.Start();

                using (DTTicketServiceReference.AlertManagementService client1 = new DTTicketServiceReference.AlertManagementService())
                {
                    response = client1.UpdateTicketToNOC(request);
                    if (response.Error != null)
                        Response.Write(response.Error.message);

                    //Response.Write("Output: " + response.UpdateAutoTaskIDOutput.ToString());

                }
                sw2.Stop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SiteTimeZoneInfo GetUTCTimediff(string connectionString, string TimeZone)
        {
            SiteTimeZoneInfoBL TimeZoneInfoBl = new SiteTimeZoneInfoBL("DefaultConnection");
            SiteTimeZoneInfo ClientSiteTimeZoneInfo = TimeZoneInfoBl.GetUTCTimediff("DefaultConnection", TimeZone);
            return ClientSiteTimeZoneInfo;
        }
    }
}

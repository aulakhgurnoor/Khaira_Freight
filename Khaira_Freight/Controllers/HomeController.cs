using Khaira_Freight.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Web.WebPages;

namespace Khaira_Freight.Controllers
{
    public class HomeController : Controller
    {
        private KhairaFreightEntities db = new KhairaFreightEntities();
        public ActionResult Index()
        {
            return View();
        }
     

        public ActionResult Apply()
        {
            ViewBag.Message = "Your apply page.";
            return View();
        }

        public ActionResult DriverApplication()
        {
            return View(GetFiles());
        }
        [HttpPost]
        public ActionResult DriverApplication(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
                string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "INSERT INTO driver_application VALUES (@Name, @ContentType, @Data)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                        cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                        cmd.Parameters.AddWithValue("@Data", bytes);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    ViewBag.Success = "Upload Successful";

                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please select file";
            }

            return View(GetFiles());
        }
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM driver_application WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }

            return File(bytes, contentType, fileName);
        }

        private static List<driver_application> GetFiles()
        {
            List<driver_application> files = new List<driver_application>();
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM driver_application"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new driver_application
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }

        //---------------------- Login-------------------------------
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserTable user)
        {
            KhairaFreightEntities usersEntities = new KhairaFreightEntities();
            Validate_UserTable_Result roleUser = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();
            //get user profile------------------------------------------------------
            List <UserTable> userList = usersEntities.UserTables.Where(x => x.Username.Equals(user.Username)).ToList();
            int roleId = Convert.ToInt32(userList[0].RoleId);
            int EmpId= Convert.ToInt32(userList[0].EmpId);
            string uname  = (userList[0].Username);

            Session["EmpId"] = EmpId;

            //---------------------------------------------------------------------------
            string message = string.Empty;
            switch (roleUser.UserId.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(2880), user.RememberMe, roleUser.Roles, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(cookie);
                    //if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                    //{

                    //    return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[2]);
                        
                    //}
                    //else
                    //{

                  //{
                        return RedirectToAction("Profile"); 
       
            }

            ViewBag.Message = message;
            return View(user);
        }

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser");
        }

        [AllowAnonymous]
        public ActionResult LoginTemp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginTemp(UserTable user)
        {
            KhairaFreightEntities usersEntities = new KhairaFreightEntities();
       
            
                Validate_UserTable_Result roleUser = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();

                string message = string.Empty;
                switch (roleUser.UserId.Value)
                {
                    case -1:
                        message = "Username and/or password is incorrect.";
                        break;
                    case -2:
                        message = "Account has not been activated.";
                        break;
                    default:
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(2880), user.RememberMe, roleUser.Roles, FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }
                        Response.Cookies.Add(cookie);
                        //if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                        //{

                        //    return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[2]);

                        //}
                        //else
                        //{

                        //{

                        //get user profile------------------------------------------------------
                        List<UserTable> userList = usersEntities.UserTables.Where(x => x.Username.Equals(user.Username)).ToList();
                        int roleId = Convert.ToInt32(userList[0].RoleId);
                        int EmpId = Convert.ToInt32(userList[0].EmpId);
                        string uname = (userList[0].Username);

                        Session["EmpId"] = EmpId;

                        //---------------------------------------------------------------------------

                        return RedirectToAction("Profile");

                }


                ViewBag.Message = message;
            
            return View(user);
        }
        //---------------------- Login-------------------------------
        [AllowAnonymous]
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginUser(UserTable user)
        {
            KhairaFreightEntities usersEntities = new KhairaFreightEntities();


            Validate_UserTable_Result roleUser = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();

            string message = string.Empty;
            switch (roleUser.UserId.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Username,  DateTime.Now, DateTime.Now.AddMinutes(2880), user.RememberMe, roleUser.Roles, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
           
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    FormsAuthentication.SetAuthCookie(user.Username,user.RememberMe);
                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(cookie);
                    //if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                    //{

                    //    return RedirectToAction(Request.Form["ReturnUrl"].Split('/')[2]);

                    //}
                    //else
                    //{

                    //{

                    //get user profile------------------------------------------------------
                    List<UserTable> userList = usersEntities.UserTables.Where(x => x.Username.Equals(user.Username)).ToList();
                    int roleId = Convert.ToInt32(userList[0].RoleId);
                    int EmpId = Convert.ToInt32(userList[0].EmpId);
                    string uname = (userList[0].Username);

                    Session["EmpId"] = EmpId;

                    //---------------------------------------------------------------------------

                    return RedirectToAction("Profile");

            }


            ViewBag.Message = message;

            return View(user);
        }

        /*Jasandeep Kaur starting point*/
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

     
        public ActionResult Team()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /*Ending point*/

        public ActionResult Contact()
        {
            feedback obj = new feedback();

            obj.added_on = DateTime.Now;

            return View(obj);
            //return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(feedback obj)

        {

            if (ModelState.IsValid)
            {
  
                db.feedbacks.Add(obj);
                db.SaveChanges();
                ViewBag.Msg = "Feedback submitted succesfully";
                return RedirectToAction("Contact");

            }

            return View(obj);
        }

        public ActionResult GetQuote()
        {
            quote obj = new quote();
          
            List<SelectListItem> equipmentList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dryvan" ,Value="Dryvan" },
                new SelectListItem() {Text="Reefer" ,Value="Reefer" },
                new SelectListItem() {Text="Flatbed" ,Value="Flatbed" },
                new SelectListItem() {Text="Stepdeck" ,Value="Stepdeck" }
                
            };

            obj.equipmentList = equipmentList;
            List<SelectListItem> provinceList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Alberta - AB" ,Value="AB" },
                new SelectListItem() {Text="British Columbia - BC" ,Value="BC" },
                new SelectListItem() {Text="Manitoba - MB" ,Value="MB" },
                new SelectListItem() {Text="New Brunswick - NB" ,Value="NB" },
                new SelectListItem() {Text="Newfoundland and Labrador - NL" ,Value="NL" },
                new SelectListItem() {Text="Northwest Territories - NT" ,Value="NT" },
                new SelectListItem() {Text="Nova Scotia - NS" ,Value="NS" },
                new SelectListItem() {Text="Nunavut - NU" ,Value="NU" },
                new SelectListItem() {Text="Ontario - ON" ,Value="ON" },
                new SelectListItem() {Text="Prince Edward Island - PE" ,Value="PE" },
                new SelectListItem() {Text="Quebec - QC" ,Value="QC" },
                new SelectListItem() {Text="Saskatchewan - SK" ,Value="SK" },
                  new SelectListItem() {Text="Yukon - YT" ,Value="YT" }
            };

            obj.provinceList = provinceList;
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetQuote(quote obj)
        {
            List<SelectListItem> equipmentList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Dryvan" ,Value="Dryvan" },
                new SelectListItem() {Text="Reefer" ,Value="Reefer" },
                new SelectListItem() {Text="Flatbed" ,Value="Flatbed" },
                new SelectListItem() {Text="Stepdeck" ,Value="Stepdeck" }

            };
            List<SelectListItem> provinceList = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Alberta - AB" ,Value="AB" },
                new SelectListItem() {Text="British Columbia - BC" ,Value="BC" },
                new SelectListItem() {Text="Manitoba - MB" ,Value="MB" },
                new SelectListItem() {Text="New Brunswick - NB" ,Value="NB" },
                new SelectListItem() {Text="Newfoundland and Labrador - NL" ,Value="NL" },
                new SelectListItem() {Text="Northwest Territories - NT" ,Value="NT" },
                new SelectListItem() {Text="Nova Scotia - NS" ,Value="NS" },
                new SelectListItem() {Text="Nunavut - NU" ,Value="NU" },
                new SelectListItem() {Text="Ontario - ON" ,Value="ON" },
                new SelectListItem() {Text="Prince Edward Island - PE" ,Value="PE" },
                new SelectListItem() {Text="Quebec - QC" ,Value="QC" },
                new SelectListItem() {Text="Saskatchewan - SK" ,Value="SK" },
                  new SelectListItem() {Text="Yukon - YT" ,Value="YT" }
            };

            obj.provinceList = provinceList;

            obj.equipmentList = equipmentList;
            if (obj.pickup_date != null && obj.drop_date != null)
            {
                if (!(obj.pickup_date >= DateTime.Now))
                {
                    ModelState.AddModelError("PickUp Date", "PickUp Date cannot be before current date");
                }
                if (!(obj.drop_date >= DateTime.Now))
                {
                    ModelState.AddModelError("Drop Date", "Drop Date cannot be before current date");
                }
                if (!(obj.drop_date >= obj.drop_date))
                {
                    ModelState.AddModelError("Drop Date", "Drop Date cannot be before PickUp Date");
                }
            }
            if (ModelState.IsValid)
            {

                db.quotes.Add(obj);
                db.SaveChanges();
                return RedirectToAction("GetQuote");

            }

            return View(obj);
        }

    }
}